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
using Chaos.Scripting.FunctionalScripts.ApplyDamage;
using Chaos.Time;
using Chaos.Time.Abstractions;
using static Chaos.Scripting.Components.EffectComponents.NonOverwritableEffectComponent;

namespace Chaos.Scripting.EffectScripts.DamageOverTimeEffects;

/// <summary>
///  The healing power will be roughly half the equivalent level of ioc
/// </summary>
public sealed class CursedTune1Effect : ContinuousAnimationEffectBase,
                                        INonOverwritableEffectComponentOptions,
                                        DamageAbilityComponent.IDamageComponentOptions,
                                        GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions
{
    public CursedTune1Effect()
    {
        ApplyDamageScript = ApplyAttackDamageScript.Create(); 
        SourceScript = this;
        AbilityTemplateKey = "cursed_tune_1";
    }
    
    private Creature _source;
    
    public List<string> ConflictingEffectNames { get; init; } =  [
        "cursed tune 1", "cursed tune 2", "cursed tune 3", "cursed tune 4",
    ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(12);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new();

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromSeconds(3));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromSeconds(3));

    /// <inheritdoc />
    public override byte Icon => 141;

    /// <inheritdoc />
    public override string Name => "cursed tune 1";
    
    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        new ComponentExecutor(_source, Subject)
            .WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<DamageAbilityComponent>();
    }

    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source; 
        return base.ShouldApply(source, target);
    }

    public IApplyDamageScript ApplyDamageScript { get; init; }
    public int? BaseDamage { get; init; } = 50;
    public Stat? DamageStat { get; init; } = Stat.INT;
    public decimal? DamageStatMultiplier { get; init; } = 3;
    public Element? Element { get; init; }
    public decimal? PctHpDamage { get; init; }
    public IScript SourceScript { get; init; }
    public decimal? PAtkMultiplier { get; init; }
    public bool? UseMatk { get; init; } = true;
    public int? FistBonus { get; init; }
    public string? AbilityTemplateKey { get; init; }
    public bool? IsSpell { get; init; } = true;
    public bool ExcludeSourcePoint { get; init; }
    public TargetFilter Filter { get; init; }
    public bool MustHaveTargets { get; init; } = false;
    public int Range { get; init; } = 8;
    public AoeShape Shape { get; init; }
    public bool SingleTarget { get; init; } = true;
}