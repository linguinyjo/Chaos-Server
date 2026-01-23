using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

/// <summary>
/// Gives the player a barrier which scales off the caster's wisdom and maximum mp.
/// The barrier can not exceed the targets maximum hp.
/// </summary>
public sealed class SolasBuffEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    private Creature _source;

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(15);

    /// <inheritdoc />
    public int? ExclusionRange { get; init; }

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
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; }

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
    [
        "solas",
    ];

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
        var barrierAmount = CalculateBarrierAmount(
            _source.StatSheet.EffectiveWis,
            _source.StatSheet.EffectiveMaximumMp,
            Subject.StatSheet.EffectiveMaximumHp
        );

        Subject.StatSheet.AddBarrier(barrierAmount);
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source;
        return base.ShouldApply(source, target);
    }

    /// <inheritdoc />
    public byte? Sound { get; init; }

    private static int CalculateBarrierAmount(
        int wisdom,
        uint maxMana,
        uint targetMaxHp)
    {
        const int wisdomSoftCap = 100;

        const float minWisdomMultiplier = 1.0f;
        const float maxWisdomMultiplier = 2.2f;

        const float manaScaling = 0.50f; // 50% of max mana
        const float hpCapPercent = 1.0f; // 100% of target HP

        // Diminishing returns wisdom curve
        var wisdomFactor = wisdom / (wisdom + (float)wisdomSoftCap);

        var wisdomMultiplier =
            minWisdomMultiplier +
            (maxWisdomMultiplier - minWisdomMultiplier) * wisdomFactor;

        var rawBarrier = maxMana * manaScaling * wisdomMultiplier;
        var maxAllowedBarrier = targetMaxHp * hpCapPercent;

        return (int)Math.Min(rawBarrier, maxAllowedBarrier);
    }
}