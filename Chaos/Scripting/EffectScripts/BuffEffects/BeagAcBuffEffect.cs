using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

public sealed class BeagAcBuffEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    /// <inheritdoc />
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; } 

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
        [
            "Armachd",
        ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(5);

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
    public override byte Icon { get; }

    /// <inheritdoc />
    public override string Name => "Armachd";
    
    private int AcBuff => 5;

    public override void OnTerminated()
    {
        Subject.StatSheet.AddBonus(new Attributes { Ac = AcBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        new ComponentExecutor(Subject, Subject).WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<AnimationAbilityComponent>()
            .Execute<SoundAbilityComponent>();
            
        Subject.StatSheet.SubtractBonus(new Attributes { Ac = AcBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        var execution = new ComponentExecutor(source, target)
            .WithOptions(this)
            .ExecuteAndCheck<NonOverwritableEffectComponent>();

        return execution is not null;
    }
}
