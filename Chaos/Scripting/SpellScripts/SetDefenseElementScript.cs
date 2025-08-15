using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.SpellScripts.Abstractions;

namespace Chaos.Scripting.SpellScripts;

public class SetDefenseElementScript : ConfigurableSpellScriptBase,
    GenericAbilityComponent<Monster>.IAbilityComponentOptions,
    SetDefenseElementAbilityComponent.ISetDefenseElementComponentOptions,
    AbilityLevellingAbilityComponent.IAbilityLevellingComponentOptions
{
    /// <inheritdoc />
    public SetDefenseElementScript(Spell subject)
        : base(subject)
    {
        IsSpell = true;
    }

    /// <inheritdoc />
    public override void OnUse(SpellContext context)
        => new ComponentExecutor(context).WithOptions(this)
            .ExecuteAndCheck<GenericAbilityComponent<Monster>>()
            ?.Execute<AbilityLevellingAbilityComponent>()
            .Execute<SetDefenseElementAbilityComponent>();

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

    /// <inheritdoc />
    public AbilityLevellingRate? LevelUpRate { get; init; }

    public string? AbilityTemplateKey { get; init; }
    public bool? IsSpell { get; init; }

    /// <inheritdoc />
    public Element Element { get; init; }

    /// <inheritdoc />
    public bool CanResist { get; init; }

    #endregion
}