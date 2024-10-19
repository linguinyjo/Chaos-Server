using Chaos.Common.Definitions;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.monkEffects;

public sealed class DracoStanceEffect : IntervalEffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(30);
    
    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(3000));

    /// <inheritdoc />
    public override byte Icon => 146;

    /// <inheritdoc />
    public override string Name => "Regeneration";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        //the interval is 100ms, so this will be applied 10 times a second
        var healPerTick = (Subject.StatSheet.EffectiveMaximumHp * 0.10);

        Subject.StatSheet.AddHp((int)Math.Ceiling(healPerTick));

        //if the subject was a player, update their vit
        AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }
}