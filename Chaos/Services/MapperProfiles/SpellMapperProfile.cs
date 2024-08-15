using Chaos.Common.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Networking.Entities.Server;
using Chaos.Schemas.Aisling;
using Chaos.Schemas.Templates;
using Chaos.Scripting.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.TypeMapper.Abstractions;

namespace Chaos.Services.MapperProfiles;

public sealed class SpellMapperProfile(ISimpleCache simpleCache, IScriptProvider scriptProvider, ITypeMapper mapper)
    : IMapperProfile<Spell, SpellSchema>, IMapperProfile<Spell, SpellInfo>, IMapperProfile<SpellTemplate, SpellTemplateSchema>
{
    private readonly ITypeMapper Mapper = mapper;
    private readonly IScriptProvider ScriptProvider = scriptProvider;
    private readonly ISimpleCache SimpleCache = simpleCache;

    public Spell Map(SpellInfo obj) => throw new NotImplementedException();

    SpellInfo IMapperProfile<Spell, SpellInfo>.Map(Spell obj)
        => new()
        {
            CastLines = obj.CastLines,
            Name = obj.Template.Name,
            PanelName = obj.PanelDisplayName,
            Prompt = obj.Template.Prompt ?? string.Empty,
            Slot = obj.Slot,
            SpellType = obj.Template.SpellType,
            Sprite = obj.Template.PanelSprite
        };

    public Spell Map(SpellSchema obj)
    {
        var template = SimpleCache.Get<SpellTemplate>(obj.TemplateKey);
        var maxLevel = template.LevelsUp ? obj.MaxLevel ?? template.MaxLevel : template.MaxLevel;
        var level = template.LevelsUp ? obj.Level ?? 0 : maxLevel;

        var spell = new Spell(
            template,
            ScriptProvider,
            obj.ScriptKeys,
            obj.UniqueId,
            obj.ElapsedMs)
        {
            Slot = obj.Slot ?? 0,
            MaxLevel = maxLevel,
            Level = level
        };

        return spell;
    }

    public SpellSchema Map(Spell obj)
    {
        var extraScriptKeys = obj.ScriptKeys
                                 .Except(obj.Template.ScriptKeys)
                                 .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var ret = new SpellSchema
        {
            UniqueId = obj.UniqueId,
            ElapsedMs = obj.Elapsed.HasValue ? Convert.ToInt32(obj.Elapsed.Value.TotalMilliseconds) : null,
            ScriptKeys = extraScriptKeys.Any() ? extraScriptKeys : null,
            TemplateKey = obj.Template.TemplateKey,
            Slot = obj.Slot,
            Level = obj.Template.LevelsUp ? obj.Level : null,
            MaxLevel = obj.Template.LevelsUp && (obj.MaxLevel != obj.Template.MaxLevel) ? obj.MaxLevel : null
        };

        return ret;
    }

    public SpellTemplate Map(SpellTemplateSchema obj)
        => new()
        {
            TemplateKey = obj.TemplateKey,
            Name = obj.Name,
            ScriptKeys = new HashSet<string>(obj.ScriptKeys, StringComparer.OrdinalIgnoreCase),
            CastLines = obj.CastLines,
            Prompt = obj.Prompt,
            SpellType = obj.SpellType,
            Cooldown = obj.CooldownMs == null ? null : TimeSpan.FromMilliseconds(obj.CooldownMs.Value),
            PanelSprite = obj.PanelSprite,
            ScriptVars = new Dictionary<string, IScriptVars>(
                obj.ScriptVars.Select(kvp => new KeyValuePair<string, IScriptVars>(kvp.Key, kvp.Value)),
                StringComparer.OrdinalIgnoreCase),
            Description = obj.Description,
            LearningRequirements = obj.LearningRequirements == null ? null : Mapper.Map<LearningRequirements>(obj.LearningRequirements),
            Level = obj.Level,
            AbilityLevel = obj.AbilityLevel,
            Class = obj.Class,
            AdvClass = obj.AdvClass,
            RequiresMaster = obj.RequiresMaster,
            LevelsUp = obj.LevelsUp,
            MaxLevel = obj.MaxLevel ?? 100,
        };

    public SpellTemplateSchema Map(SpellTemplate obj) => throw new NotImplementedException();
}