using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts;

/** Percent based damage effect */
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
        if (Subject.StatSheet.CurrentHp <= 1) return;
        var damagePerTick = (int)(Subject.StatSheet.EffectiveMaximumHp * 0.03);
        if (Subject.StatSheet.CurrentHp <= damagePerTick)
        {
            Subject.StatSheet.SetHp(1);
            return;
        }

        if (Subject.StatSheet.TrySubtractHp(damagePerTick))
            AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }
}