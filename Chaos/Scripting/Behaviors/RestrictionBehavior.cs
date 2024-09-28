using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;

namespace Chaos.Scripting.Behaviors;

public class RestrictionBehavior
{
    private readonly List<string> _playerNoMoveConditions = ["skulled", "beagpramh", "pramh", "morpramh", "ardpramh", "suain"];
    private readonly List<string> _monsterNoMoveConditions = ["skulled", "beagpramh", "pramh", "morpramh", "ardpramh", "suain",
        "beagdall", "dall", "mordall", "arddall"];
    
    public virtual bool CanMove(Creature creature)
    {
        if (creature is Aisling)
        {
            return !_playerNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
        }
        return !_monsterNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
    }

    public virtual bool CanTalk(Creature creature) => creature.IsAlive;

    public virtual bool CanTurn(Creature creature) 
    {
        if (creature is Aisling)
        {
            return !_playerNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
        }
        return !_monsterNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
    }

    public virtual bool CanUseItem(Aisling aisling, Item item) => aisling.IsAlive;

    public virtual bool CanUseSkill(Creature creature, Skill skill)
    {
        return !creature.IsAsleep(out _) && !creature.IsFrozen() && creature.IsAlive;
    }

    public virtual bool CanUseSpell(Creature creature, Spell spell)
    {
        if (creature.IsAsleep(out _) && spell.Template.Name != "ao pramh") return false;
        if (creature.IsFrozen() && spell.Template.Name != "ao suain") return false;
        return creature.IsAlive;   
    }
}