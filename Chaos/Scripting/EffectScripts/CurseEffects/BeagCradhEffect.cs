using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.CurseEffects;

public class BeagCradhEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; } = new()
    {
        TargetAnimation = 45,
        AnimationSpeed = 100
    };

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
        [
            "Beag Cradh",
            "Bad Curse",
            "Curse",
            "Not So Bad Curse"
        ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

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
    public byte? Sound { get; init; } = 27;

    /// <inheritdoc />
    public override byte Icon => 5;

    /// <inheritdoc />
    public override string Name => "Beag Cradh";

    /// <inheritdoc />
    public int AcDeduction { get; init; } = 10;

    public override void OnTerminated()
    {
        Subject.StatSheet.AddBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        new ComponentExecutor(Subject, Subject).WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<AnimationAbilityComponent>()
            .Execute<SoundAbilityComponent>();
            
        Subject.StatSheet.SubtractBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        var execution = new ComponentExecutor(source, target).WithOptions(this)
            ?.ExecuteAndCheck<NonOverwritableEffectComponent>();

        return execution is not null;
    }
}
