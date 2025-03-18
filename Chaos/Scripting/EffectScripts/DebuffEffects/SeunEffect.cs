using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.DebuffEffects;

/// <summary>
///  This works because of RelationshipBehavior which checks for the seun effect
/// </summary>
public class SeunEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions,
    GetTargetsAbilityComponent<Creature>.IGetTargetsComponentOptions,
    AnimationAbilityComponent.IAnimationComponentOptions,
    SoundAbilityComponent.ISoundComponentOptions
{
    private Creature _source;

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
            "Beag Seun"
        ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(45);

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
    public override byte Icon => 20;

    /// <inheritdoc />
    public override string Name => "Seun";

    private const int MaxLevel = 70;
    private const int EnchanterEnmity = 1000;

    public override void OnTerminated()
    {
        if (Subject is not Monster monster) return;
        var location = new Location(monster.MapInstance.Name, monster.X, monster.Y);
        var monsterTargets = Subject.MapInstance.GetEntitiesWithinRange<Monster>(location, 20);
        // This clears the monster from all other monster's aggro tables because it is no longer considered hostile
        foreach (var target in monsterTargets)
        {
            target.AggroList.TryRemove(monster.Id, out _);
        }
        monster.ResetAggro();
        monster.AggroList.AddOrUpdate(_source.Id, _ => 
            EnchanterEnmity, (_, currentAggro) => currentAggro + EnchanterEnmity);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        new ComponentExecutor(Subject, Subject).WithOptions(this)
            .ExecuteAndCheck<GetTargetsAbilityComponent<Creature>>()
            ?.Execute<AnimationAbilityComponent>()
            .Execute<SoundAbilityComponent>();
        if (Subject is Monster monster)
        {
            monster.AggroList.Clear();
        }
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        _source = source;
        return target.StatSheet.Level <= MaxLevel && base.ShouldApply(source, target);
    }
}
