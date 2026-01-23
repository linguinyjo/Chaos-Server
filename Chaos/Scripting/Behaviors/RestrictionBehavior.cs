#region
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
#endregion

namespace Chaos.Scripting.Behaviors;

public class RestrictionBehavior
{
    private readonly List<string> _playerNoMoveConditions = ["skulled", "beagpramh", "pramh", "morpramh", "ardpramh", 
        "suain"];
    private readonly List<string> _monsterNoMoveConditions = ["skulled", "beagpramh", "pramh", "morpramh", "ardpramh", "suain",
        "beagdall", "dall", "mordall", "arddall"];
    private readonly List<string> _monsterNoTurnConditions = ["skulled", "beagpramh", "pramh", "morpramh", "ardpramh", 
        "suain"];

    public virtual bool CanDropItem(Aisling aisling, Item item) => aisling.IsAlive;

    public virtual bool CanDropItemOn(Aisling aisling, Item item, Creature target) => aisling.IsAlive;

    public virtual bool CanDropMoney(Aisling aisling, int amount) => aisling.IsAlive;

    public virtual bool CanDropMoneyOn(Aisling aisling, int amount, Creature target) => aisling.IsAlive;

    public virtual bool CanMove(Creature creature)
    {
        if (creature is Aisling)
        {
            return !_playerNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
        }
        return !_monsterNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
    }

    public virtual bool CanPickupItem(Aisling aisling, GroundItem groundItem) => aisling.IsAlive;

    public virtual bool CanPickupMoney(Aisling aisling, Money money) => aisling.IsAlive;

    public virtual bool CanTalk(Creature creature) => true;

    public virtual bool CanTurn(Creature creature) 
    {
        if (creature is Aisling)
        {
            return !_playerNoMoveConditions.Any(condition => creature.Effects.Contains(condition));
        }
        return !_monsterNoTurnConditions.Any(condition => creature.Effects.Contains(condition));
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