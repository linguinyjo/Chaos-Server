using Chaos.Common.Definitions;
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
    public bool AnimatePoints { get; init; }

    /// <inheritdoc />
    public Animation? Animation { get; init; }

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } = ["Dubh Bas"];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(6);

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
    public bool SingleTarget { get; init; }

    /// <inheritdoc />
    public byte? Sound { get; init; } = 27;

    /// <inheritdoc />
    public override byte Icon => 177;

    /// <inheritdoc />
    public override string Name => "Dubh Bas";
    
    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractHp((int)Subject.StatSheet.EffectiveMaximumHp);
        AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);

        if (!Subject.IsAlive)
            Subject.Script.OnDeath();
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        new ComponentExecutor(Subject, Subject).WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>();
        AislingSubject?.SendOrangeBarMessage($"You have been marked for death");
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        if (AislingSubject?.Equipment.ContainsByTemplateKey("silverFurArmor") == true) return false;
        var execution = new ComponentExecutor(source, target).WithOptions(this)
            .ExecuteAndCheck<NonOverwritableEffectComponent>();

        return execution is not null;
    }
}
