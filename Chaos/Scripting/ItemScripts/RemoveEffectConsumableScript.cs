using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.ItemScripts;

public class RemoveEffectConsumableScript : ConfigurableItemScriptBase,
    GenericAbilityComponent<Aisling>.IAbilityComponentOptions,
    RemoveEffectAbilityComponent.IRemoveEffectComponentOptions,
    ConsumableAbilityComponent.IConsumableComponentOptions
{
    /// <inheritdoc />
    public RemoveEffectConsumableScript(Item subject, IEffectFactory effectFactory)
        : base(subject)
    {
        SourceScript = this;
        ItemName = Subject.DisplayName;
        Item = subject;
        EffectFactory = effectFactory;
    }

    /// <inheritdoc />
    public override void OnUse(Aisling source)
        => new ComponentExecutor(source, source).WithOptions(this)
            .ExecuteAndCheck<GenericAbilityComponent<Aisling>>()
            ?.Execute<ConsumableAbilityComponent>()
            .Execute<RemoveEffectAbilityComponent>();

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
    public int? ExclusionRange { get; init; }

    /// <inheritdoc />
    public bool MustHaveTargets { get; init; }

    /// <inheritdoc />
    public byte? Sound { get; init; }

    /// <inheritdoc />
    public BodyAnimation BodyAnimation { get; init; }

    public bool? ScaleBodyAnimationSpeedByAttackSpeed { get; init; }

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

    /// <inheritdoc />
    public string ItemName { get; init; }

    /// <inheritdoc />
    public bool CanResist { get; init; }

    public Item Item { get; init; }
    public IEffectFactory EffectFactory { get; init; }
    public string? EffectKey { get; init; }

    #endregion
}