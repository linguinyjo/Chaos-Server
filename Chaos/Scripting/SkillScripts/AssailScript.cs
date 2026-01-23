using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.ApplyDamage;
using Chaos.Scripting.SkillScripts.Abstractions;

namespace Chaos.Scripting.SkillScripts;

public class AssailScript : ConfigurableSkillScriptBase,
    DynamicAbilityComponent<Creature>.IDynamicAbilityComponentOptions,
    DamageAbilityComponent.IDamageComponentOptions,
    AbilityLevellingAbilityComponent.IAbilityLevellingComponentOptions,
    RequireWeaponTypeAbilityComponent.IRequireWeaponTypeComponentOptions
{
    /// <inheritdoc />
    public AssailScript(Skill subject)
        : base(subject)
    {
        ApplyDamageScript = ApplyAttackDamageScript.Create();
        SourceScript = this;
        AbilityTemplateKey = subject.Template.TemplateKey;
        IsSpell = false;
    }

    /// <inheritdoc />
    public override void OnUse(ActivationContext context)
    {
        new ComponentExecutor(context).WithOptions(this)
            .ExecuteAndCheck<RequireWeaponTypeAbilityComponent>()
            ?.ExecuteAndCheck<DynamicAbilityComponent<Creature>>()
            ?.Execute<AbilityLevellingAbilityComponent>()
            .Execute<DamageAbilityComponent>();
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
    public int? ExclusionRange { get; init; }

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

    public IApplyDamageScript ApplyDamageScript { get; init; }

    /// <inheritdoc />
    public int? BaseDamage { get; init; }

    /// <inheritdoc />
    public Stat? DamageStat { get; init; }

    /// <inheritdoc />
    public decimal? DamageStatMultiplier { get; init; }

    /// <inheritdoc />
    public Element? Element { get; init; }

    /// <inheritdoc />
    public decimal? PctHpDamage { get; init; }

    public IScript SourceScript { get; init; }

    /// <inheritdoc />
    public int? ManaCost { get; init; }

    /// <inheritdoc />
    public decimal PctManaCost { get; init; }

    /// <inheritdoc />
    public bool ShouldNotBreakHide { get; init; }

    /// <inheritdoc />
    public decimal? PAtkMultiplier { get; init; }

    /// <inheritdoc />
    public bool? UseMatk { get; init; }

    /// <inheritdoc />
    public int? FistBonus { get; init; }

    /// <inheritdoc />
    public bool CanResist { get; init; }

    /// <inheritdoc />
    public AbilityLevellingRate? LevelUpRate { get; init; }

    /// <inheritdoc />
    public string? WeaponCategory { get; init; }

    public string? AbilityTemplateKey { get; init; }
    public bool? IsSpell { get; init; }

    #endregion
}