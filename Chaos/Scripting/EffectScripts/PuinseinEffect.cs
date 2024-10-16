using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts;

public class PuinseinEffect : ContinuousAnimationEffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);

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

    /// <inheritdoc />
    public override byte Icon => 35;

    /// <inheritdoc />
    public override string Name => "Puinsein";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        var damagePerTick = (int)(Subject.StatSheet.EffectiveMaximumHp * 0.03);
        
        if (Subject.StatSheet.CurrentHp <= damagePerTick)
        {
            Subject.StatSheet.TrySubtractHp(Subject.StatSheet.CurrentHp - 1);
            return;
        }

        if (Subject.StatSheet.TrySubtractHp(damagePerTick))
            AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }
}