using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.cc;

public class DallEffect : ContinuousAnimationEffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(18000);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 42
    };

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    public override byte Icon => 3;
    
    private const byte Sound = 8;

    /// <inheritdoc />
    public override string Name => "dall";
    
    public override void OnApplied()
    {
        AislingSubject?.SetVision(VisionType.TrueBlind);
        base.OnApplied();
    }

    public override void OnTerminated()
    {
        AislingSubject?.SetVision(VisionType.Normal);
        base.OnTerminated();
    }

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        AislingSubject?.Client.SendSound(Sound, false); 
    }
}