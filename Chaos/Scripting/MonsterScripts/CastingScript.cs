using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Scripting.SpellScripts;
using Namotion.Reflection;

namespace Chaos.Scripting.MonsterScripts;

// ReSharper disable once ClassCanBeSealed.Global
public class CastingScript : MonsterScriptBase
{
    private readonly Monster Monster;

    /// <inheritdoc />
    public CastingScript(Monster subject)
        : base(subject)
    {
         Monster = subject;
    }

    /// <inheritdoc />
    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        if (Target is not { IsAlive: true } || !ShouldUseSpell || !Target.WithinRange(Subject))
            return;

        Spells.ShuffleInPlace();
       
        var spell = Spells
            .Where(spell => 
                Subject.CanUse(spell, Target, null, out _) && !EffectAlreadyOnTarget(spell, Target) &&
                    IsNonElemental(spell.Template.Name) || 
                    SpellMatchesElement(spell.Template.Name, Monster.StatSheet.OffenseElement)
                ).PickRandomWeightedSingle(1);
        if (spell is null || !Subject.TryUseSpell(spell, Target.Id)) return;
        Subject.WanderTimer.Reset();
        Subject.MoveTimer.Reset();
        Subject.SkillTimer.Reset();
    }

    /** If the spell is an effect and the target already has that effect filter it out */
    private static bool EffectAlreadyOnTarget(Spell spell, Creature target)
    {
        var effect = spell.Script.As<ApplyEffectScript>();
        return effect?.EffectKey is not null && target.Effects.Contains(effect.EffectKey);
    }

    /** Match offensive elemental spells to the monsters offensive element */
    private static bool SpellMatchesElement(string spellName, Element element)
    {
        switch (element)
        {
            case Element.Water:
                return spellName.Contains("sal", StringComparison.OrdinalIgnoreCase);
            case Element.Wind:
                return spellName.Contains("athar", StringComparison.OrdinalIgnoreCase);
            case Element.Earth:
                return spellName.Contains("creag", StringComparison.OrdinalIgnoreCase);
            case Element.Fire:
                return spellName.Contains("srad", StringComparison.OrdinalIgnoreCase);
            default:
                return false;
        }
    }
    
    private static bool IsNonElemental(string spellName)
    {
        // Check if the spell name doesn't contain any elemental keyword
        return !spellName.Contains("sal", StringComparison.OrdinalIgnoreCase) && 
               !spellName.Contains("athar", StringComparison.OrdinalIgnoreCase) && 
               !spellName.Contains("creag", StringComparison.OrdinalIgnoreCase) && 
               !spellName.Contains("srad", StringComparison.OrdinalIgnoreCase);
    }
}