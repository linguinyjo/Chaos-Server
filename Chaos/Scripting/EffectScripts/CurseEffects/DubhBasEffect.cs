using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.CurseEffects;

public class DubhBasEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(6);

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
    public bool SingleTarget { get; init; }

    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; }

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } = ["Dubh Bas"];

    /// <inheritdoc />
    public override byte Icon => 177;

    /// <inheritdoc />
    public override string Name => "Dubh Bas";

    public override void OnTerminated()
    {
        Subject.StatSheet.SetHp(1);
        AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        AislingSubject?.SendOrangeBarMessage("You feel a darkness seeping through you veins");
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        return AislingSubject?.Equipment.ContainsByTemplateKey("silverFurTarp") != true &&
               base.ShouldApply(source, target);
    }

    /// <inheritdoc />
    public byte? Sound { get; init; }
}