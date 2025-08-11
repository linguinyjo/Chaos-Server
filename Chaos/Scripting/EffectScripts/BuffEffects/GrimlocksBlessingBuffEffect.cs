using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

public sealed class GrimlocksBlessingBuffEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; } = new Animation()
    {
        AnimationSpeed = 150,
        TargetAnimation = 21
    };

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } = [];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(30);

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
    public byte? Sound { get; init; } = 31;

    /// <inheritdoc />
    public override byte Icon => 127;

    /// <inheritdoc />
    public override string Name => "Grimlocks blessing";
    
    private static int DamageBuff => 15;

    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { Dmg = DamageBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.AddBonus(new Attributes { Dmg = DamageBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
}
