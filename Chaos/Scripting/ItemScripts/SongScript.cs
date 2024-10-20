using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Formulae;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.ApplyDamage;
using Chaos.Scripting.FunctionalScripts.ApplyHealing;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Storage.Abstractions;
using static Chaos.Common.Definitions.Town;

namespace Chaos.Scripting.ItemScripts;

public class SongScript : ConfigurableItemScriptBase,
                                        GenericAbilityComponent<Aisling>.IAbilityComponentOptions,
                                        ConsumableAbilityComponent.IConsumableComponentOptions
{
    private readonly ISimpleCache SimpleCache;
    
    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    #endregion

    /// <inheritdoc />
    public SongScript(Item subject, ISimpleCache simpleCache)
        : base(subject)
    {
        SourceScript = this;
        Slot = Subject.Slot;
        ItemName = Subject.DisplayName;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnUse(Aisling source)
    {
        new ComponentExecutor(source, source).WithOptions(this)
            .ExecuteAndCheck<GenericAbilityComponent<Aisling>>()
            ?.Execute<ConsumableAbilityComponent>();
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        source.TraverseMap(targetMap, Destination);

        // switch (Destination)
        // {
        //     case Mileth:
        //         Console.WriteLine("You are in Mileth");
        //         var targetMap = SimpleCache.Get<MapInstance>("milethInn");
        //         var destination = new Location("milethInn", 5, 10);
        //         source.TraverseMap(targetMap, destination);
        //         break;
        //     case Abel:
        //         Console.WriteLine("You are in Abel");
        //         // Add Abel-specific logic here
        //         break;
        //     case Rucesion:
        //         Console.WriteLine("You are in Rucesion");
        //         // Add Rucesion-specific logic here
        //         break;
        //     case Piet:
        //         Console.WriteLine("You are in Piet");
        //         // Add Piet-specific logic here
        //         break;
        //     case Suomi:
        //         Console.WriteLine("You are in Suomi");
        //         // Add Suomi-specific logic here
        //         break;
        //     default:
        //         Console.WriteLine("Unknown town");
        //         // Handle unexpected cases
        //         break;
        // }
    }
    


    #region ScriptVars
    /// <inheritdoc />
    public AoeShape Shape { get; init; }

    /// <inheritdoc />
    public bool SingleTarget { get; init; }

    /// <inheritdoc />
    public TargetFilter Filter { get; init; }

    /// <inheritdoc />
    public int Range { get; init; }

    /// <inheritdoc />
    public bool ExcludeSourcePoint { get; init; }

    /// <inheritdoc />
    public bool MustHaveTargets { get; init; }

    /// <inheritdoc />
    public byte? Sound { get; init; }

    /// <inheritdoc />
    public BodyAnimation BodyAnimation { get; init; }

    /// <inheritdoc />
    public ushort? AnimationSpeed { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; }

    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public int? ManaCost { get; init; }

    /// <inheritdoc />
    public decimal PctManaCost { get; init; }

    /// <inheritdoc />
    public bool ShouldNotBreakHide { get; init; }
    
    public IScript SourceScript { get; init; }
    

    public string ItemName { get; init; }

    /// <inheritdoc />
    public byte Slot { get; init; }
    public Item Item { get; init; }
    public bool CanResist { get; init; }

    #endregion
}