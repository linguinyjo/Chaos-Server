using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects.Warrior.Champion;

public class HateAura1Effect : ContinuousAnimationEffectBase, 
                                GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
                                AnimationAuraAbilityComponent.IAnimationAuraComponentOptions,
                                TauntAbilityComponent.ITauntComponentOptions,
                                NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(180000);
    
    public List<string> ConflictingEffectNames { get; init; } =
        ["Aura of Hate 1", "Aura of Hate 2"];

    /// <inheritdoc />
    protected override Animation Animation { get; } = new();
    
    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(6000));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(6000));

    /// <inheritdoc />
    public override byte Icon => 132;
    
    private Creature? _source;
    private Creature? _target;

    /// <inheritdoc />
    public override string Name => "Aura of Hate 1";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        if (_source == null || _target == null) return;
        new ComponentExecutor(_source, _target).WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<AnimationAuraAbilityComponent>()
            .Execute<TauntAbilityComponent>();
    }
    
    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source;  
        _target = target; 
        return base.ShouldApply(source, target);
    }

    public bool ExcludeSourcePoint { get; init; } = true;
    public TargetFilter Filter { get; init; } = TargetFilter.MonstersOnly;
    public bool MustHaveTargets { get; init; } = false;
    public int Range { get; init; } = 2;
    public AoeShape Shape { get; init; } = AoeShape.AllAround;
    public bool SingleTarget { get; init; } = false;
    public int Enmity { get; init; } = 100;
    public bool AnimatePointsAura { get; init; } = false;
    public Animation? AnimationAura { get; init; } = new()
    {
        AnimationSpeed = 150,
        TargetAnimation = 134
    };
}
