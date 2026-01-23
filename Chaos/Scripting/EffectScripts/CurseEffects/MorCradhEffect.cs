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

public class MorCradhEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    private const int AcDeduction = 40;

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

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
    public Animation? Animation { get; init; } = new()
    {
        TargetAnimation = 18,
        AnimationSpeed = 150
    };

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
    [
        "Beag Cradh",
        "Cradh",
        "Mor Cradh",
        "Not So Bad Curse"
    ];

    /// <inheritdoc />
    public override byte Icon => 83;

    /// <inheritdoc />
    public override string Name => "Mor Cradh";

    public override void OnTerminated()
    {
        Subject.StatSheet.AddBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        var result = new ComponentExecutor(source, Subject).WithOptions(this)
            .ExecuteAndCheck<NonOverwritableEffectComponent>();
        return result != null;
    }

    /// <inheritdoc />
    public byte? Sound { get; init; }
}