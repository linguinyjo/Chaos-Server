using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

/// <summary>
/// Gives the player a barrier which scales off the caster's wisdom stat.
/// The maximum barrier is 45% and the minimum is 15% of the targets maximum hp.
/// </summary>
public sealed class SolasBuffEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    private Creature _source;

    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; } 

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
        [
            "solas",
        ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(60);

    /// <inheritdoc />
    public bool ExcludeSourcePoint { get; init; }

    /// <inheritdoc />
    public TargetFilter Filter { get; init; }

    /// <inheritdoc />
    public bool MustHaveTargets { get; init; }

    /// <inheritdoc />
    public int Range { get; init; }

    /// <inheritdoc />
    public AoeShape Shape { get; init; }

    /// <inheritdoc />
    public bool SingleTarget { get; init; } = true;

    /// <inheritdoc />
    public byte? Sound { get; init; }

    /// <inheritdoc />
    public override byte Icon => 53;

    /// <inheritdoc />
    public override string Name => "solas";

    public override void OnTerminated()
    {
        Subject.StatSheet.RemoveBarrier();
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        var barrierAmount = CalculateBarrierAmount(_source.StatSheet.EffectiveWis, Subject.StatSheet.EffectiveMaximumHp);
        Subject.StatSheet.AddBarrier(barrierAmount);
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source;
        return base.ShouldApply(source, target);
    }
    
    private static int CalculateBarrierAmount(int wisdom, uint maxHp)
    {
        const float minScaling = 0.15f;  // 15% barrier at base Wisdom
        const float maxScaling = 0.45f;  // 45% barrier at max Wisdom
        const int maxWisdom = 200;       // Max Wisdom for full scaling

        // Calculate scaling factor based on Wisdom progression
        var scalingFactor = minScaling + ((maxScaling - minScaling) * (wisdom / (float)maxWisdom));

        // Ensure it remains within bounds
        scalingFactor = Math.Clamp(scalingFactor, minScaling, maxScaling);

        // Calculate and return the barrier amount
        return (int)(maxHp * scalingFactor);
    }
}
