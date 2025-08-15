using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.PoisonEffects;

public class MorPoisonEffect : ContinuousAnimationEffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{
    private const int DamagePerTick = 100;

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(15);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 247
    };

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1000));

    public List<string> ConflictingEffectNames { get; init; } =
    [
        "beag poison",
        "poison",
        "mor poison"
    ];

    /// <inheritdoc />
    public override byte Icon => 35;

    /// <inheritdoc />
    public override string Name => "mor poison";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        if (Subject.StatSheet.CurrentHp <= DamagePerTick)
            return;

        if (Subject.StatSheet.TrySubtractHp(DamagePerTick))
            AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }
}