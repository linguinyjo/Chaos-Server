using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.SongEffects;

/// <summary>
///  The healing power will be roughly half the equivalent level of ioc
/// </summary>
public sealed class OranBeatha2Effect : ContinuousAnimationEffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    HealAbilityComponent.IHealComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions
{
    private Creature? _source;

    public OranBeatha2Effect()
    {
        SourceScript = this;
        AbilityTemplateKey = "oran beatha 2";
    }

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(12);

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

    public int? ExclusionRange { get; init; }
    public TargetFilter Filter { get; init; } = TargetFilter.GroupOnly;
    public bool MustHaveTargets { get; init; } = false;
    public int Range { get; init; }
    public AoeShape Shape { get; init; }
    public bool SingleTarget { get; init; }

    public IApplyHealScript ApplyHealScript { get; init; } = FunctionalScripts.ApplyHealing.ApplyHealScript.Create();
    public int? BaseHeal { get; init; } = 150;
    public Stat? HealStat { get; init; } = Stat.WIS;
    public decimal? HealStatMultiplier { get; init; } = 5;
    public decimal? MagicAttackMultiplier { get; init; } = 5;
    public decimal? PctHpHeal { get; init; }
    public string? AbilityTemplateKey { get; init; }
    public List<string> ConflictingEffectNames { get; init; } = ["oran_beatha_1", "oran_beatha_2"];

    /// <inheritdoc />
    public override byte Icon => 146;

    /// <inheritdoc />
    public override string Name => "oran beatha 2";

    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source;
        return base.ShouldApply(source, target);
    }

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        if (_source != null)
            new ComponentExecutor(_source, Subject)
                .WithOptions(this)
                .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
                ?.Execute<HealAbilityComponent>();
    }
}