using System.Collections;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using AutoMapper;
using Chaos;
using Chaos.Common.Abstractions;
using Chaos.Common.Utilities;
using Chaos.Extensions;
using Chaos.Extensions.DependencyInjection;
using Chaos.Geometry.JsonConverters;
using Chaos.Schemas.Content;
using Chaos.Schemas.Data;
using Chaos.Schemas.Templates;
using Chaos.Services.Configuration;
using Chaos.Storage;
using Chaos.Storage.Abstractions;
using Chaos.Utilities;
using ChaosTool.Model;
using ChaosTool.Model.Abstractions;
using ChaosTool.Model.Tables;
using ChaosTool.ViewModel;
using ChaosTool.ViewModel.Observables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChaosTool;

public class JsonContext
{
    private static readonly SerializationContext Context;
    private static readonly IServiceProvider Services;

    private static TaskCompletionSource LoadingCompletion;
    private static bool IsInitialized;

    private static readonly TypeSwitchExpression<IEnumerable> RepositoryExpression = new TypeSwitchExpression<IEnumerable>()
                                                                                     .Case<DialogTemplateSchema>(() => DialogTemplates)
                                                                                     .Case<ItemTemplateSchema>(() => ItemTemplates)
                                                                                     .Case<LootTableSchema>(() => LootTables)
                                                                                     .Case<MapInstanceRepository.MapInstanceComposite>(
                                                                                         () => MapInstances)
                                                                                     .Case<MapTemplateSchema>(() => MapTemplates)
                                                                                     .Case<MerchantTemplateSchema>(() => MerchantTemplates)
                                                                                     .Case<MonsterTemplateSchema>(() => MonsterTemplates)
                                                                                     .Case<ReactorTileTemplateSchema>(
                                                                                         () => ReactorTileTemplates)
                                                                                     .Case<SkillTemplateSchema>(() => SkillTemplates)
                                                                                     .Case<SpellTemplateSchema>(() => SpellTemplates)
                                                                                     .Default(() => throw new ArgumentOutOfRangeException())
                                                                                     .Freeze();

    public static DialogTemplateRepository DialogTemplates { get; private set; } = null!;
    public static ItemTemplateRepository ItemTemplates { get; private set; } = null!;
    public static Task LoadingTask { get; private set; }
    public static LootTableRepository LootTables { get; private set; } = null!;
    public static MapInstanceRepository MapInstances { get; private set; } = null!;
    public static MapTemplateRepository MapTemplates { get; private set; } = null!;
    public static MerchantTemplateRepository MerchantTemplates { get; private set; } = null!;
    public static MonsterTemplateRepository MonsterTemplates { get; private set; } = null!;
    public static ReactorTileTemplateRepository ReactorTileTemplates { get; private set; } = null!;
    public static SkillTemplateRepository SkillTemplates { get; private set; } = null!;
    public static SpellTemplateRepository SpellTemplates { get; private set; } = null!;
    public static AislingRepository Aislings { get; } = null!;
    public static string BaseDirectory { get; }
    public static JsonSerializerOptions JsonSerializerOptions { get; }

    static JsonContext()
    {
        LoadingCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        LoadingTask = LoadingCompletion.Task;

        JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            IgnoreReadOnlyProperties = true,
            IgnoreReadOnlyFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        JsonSerializerOptions.Converters.Add(new PointConverter());
        JsonSerializerOptions.Converters.Add(new LocationConverter());
        JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        Context = new SerializationContext(JsonSerializerOptions);

        var services = new ServiceCollection();

        // @formatter:off
        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            #if DEBUG
                            .AddJsonFile("appsettings.local.json")
                            #else
                            .AddJsonFile("appsettings.prod.json")
                            //.AddJsonFile("appsettings.local.json")
                            #endif
                            ;
        // @formatter:on
        var configuration = builder.Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddTypeMapper();
        AddStaticAutoMapper();
        services.AddTransient<IEntityRepository, EntityRepository>();
        services.AddLogging();

        services.AddOptions<JsonSerializerOptions>()
                .Configure<ILogger<WarningJsonTypeInfoResolver>>(
                    (options, logger) =>
                    {
                        if (!IsInitialized)
                        {
                            IsInitialized = true;
                            var defaultResolver = new WarningJsonTypeInfoResolver(logger);
                            var combinedResoler = JsonTypeInfoResolver.Combine(Context, defaultResolver);

                            JsonSerializerOptions.SetTypeResolver(combinedResoler);
                        }

                        ShallowCopy<JsonSerializerOptions>.Merge(JsonSerializerOptions, options);
                    });

        services.ConfigureOptions<OptionsConfigurer>();
        services.ConfigureOptions<OptionsValidator>();

        services.AddOptionsFromConfig<ChaosOptions>(ConfigKeys.Options.Key);

        services.AddSingleton<IStagingDirectory>(
            sp => sp.GetRequiredService<IOptions<ChaosOptions>>()
                    .Value);

        services.AddStorage();

        Services = services.BuildServiceProvider();

        var stagingDir = Services.GetRequiredService<IStagingDirectory>();
        stagingDir.StagingDirectory = Path.Combine("..", stagingDir.StagingDirectory);
        BaseDirectory = stagingDir.StagingDirectory;

        CreateTables();
    }

    private static void AddStaticAutoMapper()
        => AutoMapperEx.Initialize(
            cfg =>
            {
                //remove CRLF line endings and replace with LF
                cfg.ValueTransformers.Add(
                    new ValueTransformerConfiguration(typeof(string), (string? str) => str == null ? null : str.ReplaceLineEndings("\n")));

                cfg.CreateMap<string, string>()
                   .ConvertUsing(r => r);

                cfg.CreateMap<string, BindableString>()
                   .ForMember(l => l.String, c => c.MapFrom(l => l))
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<StatsSchema, ObservableStats>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<AttributesSchema, ObservableAttributes>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<StatSheetSchema, ObservableStatSheet>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<DialogOptionSchema, ObservableDialogOption>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<ItemDetailsSchema, ObservableItemDetails>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<ItemRequirementSchema, ObservableItemRequirement>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<AbilityRequirementSchema, ObservableAbilityRequirement>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<LearningRequirementsSchema, ObservableLearningRequirements>()
                   .ForMember(l => l.RequiredStats, c => c.MapFrom(l => l.RequiredStats))
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<LootDropSchema, ObservableLootDrop>()
                   .PreserveReferences()
                   .ReverseMap();

                //TOP LEVEL
                cfg.CreateMap<DialogTemplateSchema, DialogTemplateViewModel>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<ItemTemplateSchema, ItemTemplateViewModel>()
                   .ForMember(l => l.Modifiers, c => c.MapFrom(l => l.Modifiers))
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<LootTableSchema, LootTableViewModel>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<MapTemplateSchema, MapTemplateViewModel>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<MerchantTemplateSchema, MerchantTemplateViewModel>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<MonsterTemplateSchema, MonsterTemplateViewModel>()
                   .ForMember(l => l.StatSheet, c => c.MapFrom(l => l.StatSheet))
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<ReactorTileTemplateSchema, ReactorTileTemplateViewModel>()
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<SkillTemplateSchema, SkillTemplateViewModel>()
                   .ForMember(l => l.LearningRequirements, c => c.MapFrom(l => l.LearningRequirements))
                   .PreserveReferences()
                   .ReverseMap();

                cfg.CreateMap<SpellTemplateSchema, SpellTemplateViewModel>()
                   .ForMember(l => l.LearningRequirements, c => c.MapFrom(l => l.LearningRequirements))
                   .PreserveReferences()
                   .ReverseMap();

                //TRACE WRAPPERS
                cfg.CreateMap<TraceWrapper<DialogTemplateSchema>, DialogTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<ItemTemplateSchema>, ItemTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<LootTableSchema>, LootTableViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<MapTemplateSchema>, MapTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<MerchantTemplateSchema>, MerchantTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<MonsterTemplateSchema>, MonsterTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<ReactorTileTemplateSchema>, ReactorTileTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<SkillTemplateSchema>, SkillTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();

                cfg.CreateMap<TraceWrapper<SpellTemplateSchema>, SpellTemplateViewModel>()
                   .ForMember(r => r.OriginalPath, c => c.MapFrom(l => l.Path))
                   .IncludeMembers(l => l.Object)
                   .ReverseMap();
            });

    private static void CreateTables()
    {
        LootTables = ActivatorUtilities.CreateInstance<LootTableRepository>(Services);

        //Aislings = ActivatorUtilities.CreateInstance<AislingRepository>(Services);
        MapInstances = ActivatorUtilities.CreateInstance<MapInstanceRepository>(Services);
        DialogTemplates = ActivatorUtilities.CreateInstance<DialogTemplateRepository>(Services);
        ItemTemplates = ActivatorUtilities.CreateInstance<ItemTemplateRepository>(Services);
        MapTemplates = ActivatorUtilities.CreateInstance<MapTemplateRepository>(Services);
        MerchantTemplates = ActivatorUtilities.CreateInstance<MerchantTemplateRepository>(Services);
        MonsterTemplates = ActivatorUtilities.CreateInstance<MonsterTemplateRepository>(Services);
        ReactorTileTemplates = ActivatorUtilities.CreateInstance<ReactorTileTemplateRepository>(Services);
        SkillTemplates = ActivatorUtilities.CreateInstance<SkillTemplateRepository>(Services);
        SpellTemplates = ActivatorUtilities.CreateInstance<SpellTemplateRepository>(Services);
    }

    public static RepositoryBase<TSchema> GetRepository<TSchema>() where TSchema: class
    {
        var repository = RepositoryExpression.Switch<TSchema>();

        return (RepositoryBase<TSchema>)repository!;
    }

    internal static async Task LoadAsync()
    {
        await Task.WhenAll(
            LootTables.LoadAsync(),

            //Aislings.LoadAsync(),
            MapInstances.LoadAsync(),
            DialogTemplates.LoadAsync(),
            ItemTemplates.LoadAsync(),
            MapTemplates.LoadAsync(),
            MerchantTemplates.LoadAsync(),
            MonsterTemplates.LoadAsync(),
            ReactorTileTemplates.LoadAsync(),
            SkillTemplates.LoadAsync(),
            SpellTemplates.LoadAsync());

        LoadingCompletion.TrySetResult();
    }

    internal static async Task ReloadAsync()
    {
        LoadingCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        LoadingTask = LoadingCompletion.Task;

        CreateTables();

        await LoadAsync();
    }

    public static Task SaveChangesAsync()
        => Task.WhenAll(
            LootTables.SaveChangesAsync(),
            Aislings.SaveChangesAsync(),
            MapInstances.SaveChangesAsync(),
            DialogTemplates.SaveChangesAsync(),
            ItemTemplates.SaveChangesAsync(),
            MapTemplates.SaveChangesAsync(),
            MerchantTemplates.SaveChangesAsync(),
            MonsterTemplates.SaveChangesAsync(),
            ReactorTileTemplates.SaveChangesAsync(),
            SkillTemplates.SaveChangesAsync(),
            SpellTemplates.SaveChangesAsync());
}