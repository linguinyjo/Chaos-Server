using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct CheckResistAbilityComponent : IConditionalComponent 
{
    private static readonly Animation ResistAnimation = new Animation(targetAnimation: 33, animationSpeed: 150);
    
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<ICheckResistComponentOptions>();
        if (!options.CanResist) return true;
        var targets = vars.GetTargets<Creature>();
        var resistedTargets = new List<Creature>();
        
        foreach (var target in targets)
        {
            var shouldResist = target is Aisling ? ShouldPlayerResist(target) : ShouldMonsterResist();
            if (!shouldResist) continue;
            resistedTargets.Add(target);
            target.Animate(ResistAnimation, context.Source.Id);
        }
        // Remove resisted targets from the target pool
        vars.RemoveTargetsIf<Creature>(target => resistedTargets.Contains(target));

        // If all targets resisted, return false to indicate the spell had no effect
        return targets.Count != resistedTargets.Count;
    }

    private bool ShouldMonsterResist()
    {
        return false;
    }
    
    private bool ShouldPlayerResist(Creature target)
    {
        var resistChance = target.StatSheet.EffectiveMagicResistance;
        if (resistChance > 70)
        {
            resistChance = 70;
        }
        var roll = Random.Shared.Next(1, 101);
        return roll <= resistChance;
    }
    
    public interface ICheckResistComponentOptions
    {
        bool CanResist { get; init; }
    }
}