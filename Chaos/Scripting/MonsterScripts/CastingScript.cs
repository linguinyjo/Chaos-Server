using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.Models.World;
using Chaos.Scripting.MonsterScripts.Abstractions;
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

        var spell = Spells.Where(spell => Subject.CanUse(spell, Target, null, out _)
                                                  && IsNonElemental(spell.Template.Name) || 
                                                  SpellMatchesElement(spell.Template.Name, Monster.StatSheet.OffenseElement))
            .PickRandomWeightedSingle(1);
        if (spell is null || !Subject.TryUseSpell(spell, Target.Id)) return;
        Subject.WanderTimer.Reset();
        Subject.MoveTimer.Reset();
        Subject.SkillTimer.Reset();
    }
    
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
    
    // Low
    public static T? PickRandomWeightedSingle<T>(ICollection<KeyValuePair<T, int>>? weightedChoices)
    {
        // If there are no valid choices, return default
        if (weightedChoices == null || weightedChoices.Count == 0)
            return default;

        // Calculate the total sum of all weights
        var totalWeight = weightedChoices.Sum(pair => pair.Value);

        // If total weight is 0, no selection can be made
        if (totalWeight <= 0)
            return default;

        // Roll a random number between 1 and the total weight
        var randomRoll = Random.Shared.Next(1, totalWeight + 1);

        // Iterate through the weighted choices and select one based on the random roll
        foreach (var pair in weightedChoices)
        {
            if (randomRoll <= pair.Value)
                return pair.Key;

            randomRoll -= pair.Value;
        }

        // Fallback in case no item was selected, which should not happen
        return default;
    }
}