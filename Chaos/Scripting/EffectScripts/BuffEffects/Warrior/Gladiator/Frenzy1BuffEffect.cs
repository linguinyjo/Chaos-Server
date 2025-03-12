using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects.Warrior.Gladiator;

public sealed class Frenzy1BuffEffect : EffectBase,
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
        ["frenzy 1", "frenzy 2", "frenzy 3"];

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
    public override byte Icon => 135;
    /// <inheritdoc />
    public override string Name => "frenzy 1";
    
    private int AcDebuff => 20;
    private int DmgBuff => 10;

    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractBonus(new Attributes
        {
            Ac = AcDebuff,
            Dmg = DmgBuff,
        });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.AddBonus(new Attributes
        {
            Ac = AcDebuff,
            Dmg = DmgBuff,
        });
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
