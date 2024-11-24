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
            var shouldResist = target is Aisling ? 
                ShouldPlayerResist(target) : 
                ShouldMonsterResist(context.SourceAisling, target.StatSheet.EffectiveMagicResistance);
            
            if (!shouldResist) continue;
            resistedTargets.Add(target);
            target.Animate(ResistAnimation, context.Source.Id);
        }
        // Remove resisted targets from the target pool
        vars.RemoveTargetsIf<Creature>(target => resistedTargets.Contains(target));

        // If all targets resisted, return false to indicate the spell had no effect
        return targets.Count != resistedTargets.Count;
    }

    private static bool ShouldMonsterResist(Aisling? aisling, int magicResistance)
    {
        var chanceToHit = PlayerChanceToHit(aisling?.StatSheet.EffectiveHit ?? 0, magicResistance);
        var random = new Random();
        var chance = random.Next(1, 101);
        Console.WriteLine($"Accuracy: {chanceToHit} You rolled: {chance}");
        return chance >= chanceToHit;
    }
    
    private static bool ShouldPlayerResist(Creature target)
    {
        var resistChance = target.StatSheet.EffectiveMagicResistance;
        if (resistChance > 70)
        {
            resistChance = 70;
        }
        var roll = Random.Shared.Next(1, 101);
        return roll <= resistChance;
    }
    
    // Hit capped to 40, MR capped to 70
    private static double PlayerChanceToHit(double hit, double magicResistance)
    {
        hit = Math.Min(hit, 40);
        magicResistance = Math.Min(magicResistance, 70); 
        var accuracy = 100 * (1 - Math.Max(0, (magicResistance - (hit / 40.0 * 70)) / 70.0));
        Console.WriteLine($"Accuracy: {accuracy}");
        return accuracy;
    }
    
    public interface ICheckResistComponentOptions
    {
        bool CanResist { get; init; }
    }
}

// Hit  MR	%	Explanation
// 0	10	86%	Minimal hit, resistance lowers accuracy.
// 2	10	91%	Slight improvement with hit.
// 4	10	97%	Closing in on 100% for low resistance.
// 6	10	100%	Fully negates 10% resistance.
// 0	20	71%	No hit leaves most resistance intact.
// 4	20	83%	More hit begins reducing resistance.
// 8	20	94%	Closing in on 100%.
// 11	20	100%	Fully negates 20% resistance.
// 0	30	57%	Low hit, resistance strongly impacts.
// 6	30	74%	Moderate hit improves accuracy.
// 12	30	90%	Approaching 100%.
// 17.	30	100%	Fully negates 30% resistance.
// 0	40	43%	Low accuracy against higher resistance.
// 8	40	66%	Significant improvement with hit.
// 16	40	89%	Nearing full accuracy.
// 23	40	100%	Fully negates 40% resistance.
// 0	50	29%	Very low accuracy with minimal hit.
// 10	50	58%	Moderate improvement with more hit.
// 20	50	86%	High hit strongly reduces resistance.
// 28	50	100%	Fully negates 50% resistance.
// 0	60	14%	Barely any accuracy against high MR.
// 12	60	50%	Halfway accuracy with decent hit.
// 24	60	86%	High hit mostly neutralizes resistance.
// 34	60	100%	Fully negates 60% resistance.
// 0	70	0%	No accuracy at max resistance.
// 14	70	33%	Hit begins overcoming some resistance.
// 28	70	80%	High hit strongly reduces resistance.
// 40	70	100%	Fully negates 70% resistance.