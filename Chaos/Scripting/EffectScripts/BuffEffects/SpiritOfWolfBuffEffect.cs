using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

public sealed class SpiritOfWolfBuffEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(10);

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
        "Spirit of wolf"
    ];

    /// <inheritdoc />
    public override byte Icon => 92;

    /// <inheritdoc />
    public override string Name => "Spirit of Wolf";

    public override void OnTerminated()
    {
        AislingSubject?.SetSprite(0);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        AislingSubject?.SetSprite(2);
    }

    /// <inheritdoc />
    public byte? Sound { get; init; }
}