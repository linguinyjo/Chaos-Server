using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.RegenerationEffects;

/// <summary>
///  The healing power will be roughly half the equivalent level of ioc
/// </summary>
public sealed class OranBeatha2Effect : ContinuousAnimationEffectBase,
                                        HealAbilityComponent.IHealComponentOptions,
                                        GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions
{
    public OranBeatha2Effect()
    {
        SourceScript = this;
        AbilityTemplateKey = Name;
    }

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(8);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 187
    };

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromSeconds(2));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromSeconds(2));

    /// <inheritdoc />
    public override byte Icon => 146;

    /// <inheritdoc />
    public override string Name => "oran beatha 2";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        new ComponentExecutor(Subject, Subject)
            .WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<HealAbilityComponent>();
    }

    public IApplyHealScript ApplyHealScript { get; init; } = FunctionalScripts.ApplyHealing.ApplyHealScript.Create();
    public int? BaseHeal { get; init; } = 150;
    public Stat? HealStat { get; init; } = Stat.WIS;
    public decimal? HealStatMultiplier { get; init; } = 5;
    public decimal? MagicAttackMultiplier { get; init; } = 5;
    public decimal? PctHpHeal { get; init; }
    public IScript SourceScript { get; init; } 
    public string? AbilityTemplateKey { get; init; }
    public bool ExcludeSourcePoint { get; init; }
    public TargetFilter Filter { get; init; } = TargetFilter.SelfOnly;
    public bool MustHaveTargets { get; init; } = false;
    public int Range { get; init; }
    public AoeShape Shape { get; init; }
    public bool SingleTarget { get; init; }
}