using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.DebuffEffects;

public class BeagSuainEffect : ContinuousAnimationEffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(3000);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 41
    };

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    public override byte Icon => 50;
    
    private const byte Sound = 8;

    /// <inheritdoc />
    public override string Name => "suain";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        AislingSubject?.Client.SendSound(Sound, false); 
    }

    public override bool ShouldApply(Creature source, Creature target)
    {
        if (!target.Effects.Contains("suain"))
        {
            return true;
        }

        if (source is Aisling aisling) 
        {
            aisling.SendOrangeBarMessage("Target is already under this effect");
        }

        return false;
    }
}
