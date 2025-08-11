using Chaos.Formulae.Abstractions;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;

namespace Chaos.Formulae.Experience;

public class DefaultExperienceFormula : IExperienceFormula
{
    /// <inheritdoc />
    public long Calculate(Creature killedCreature, params Aisling[] aislings)
    {
        switch (killedCreature)
        {
            case Aisling:
                return 0;
            case Monster monster:
                var groupSizeDeductions = GetGroupSizeDeductions(aislings);
                var partyLevelDifferenceDeductions = GetPartyLevelDifferenceDeductions(aislings);
                var monsterLevelDeductions = GetMonsterLevelDifferenceDeductions(aislings, monster);

                if (monsterLevelDeductions == decimal.MaxValue)
                {
                    return 1;
                }

                var groupMultiplier = Math.Max(0, 1 - (groupSizeDeductions + partyLevelDifferenceDeductions));
                var monsterLevelMultiplier = Math.Max(0, 1 - monsterLevelDeductions);

                return Convert.ToInt64(monster.Experience * groupMultiplier * monsterLevelMultiplier);
        }
        return 0;
    }

    protected virtual decimal GetGroupSizeDeductions(ICollection<Aisling> group)
        => group.Count switch
        {
            1 => 0,
            2 => 0.20m,
            3 => 0.30m,
            4 => 0.35m,
            5 => 0.40m,
            6 => 0.50m,
            7 => 0.60m,
            8 => 0.70m,
            9 => 0.80m,
            _ => 0.95m
        };

    // ReSharper disable once ParameterTypeCanBeEnumerable.Global
    protected virtual decimal GetMonsterLevelDifferenceDeductions(ICollection<Aisling> group, Monster monster)
    {
        // Dont apply any deductions to monster level 80+
        if (monster.StatSheet.Level > 80) return 0;
        var highestPlayerLevel = group.Max(p => p.StatSheet.Level);
        var monsterLevel = monster.StatSheet.Level;
        var levelDifference = highestPlayerLevel - monsterLevel;

        // If highest level player is a lower level than the moster dont apply any deductions
        if (levelDifference <= 0) return 0;

        // 25% reduction for 6 levels higher
        // 50% reduction for 7 levels higher
        return levelDifference switch
        {
            <= 5 => 0,
            6 => 0.25m,
            7 => 0.50m,
            _ => decimal.MaxValue
        };
    }

    protected virtual decimal GetPartyLevelDifferenceDeductions(ICollection<Aisling> group)
    {
        var lowestMember = group.MinBy(p => p.StatSheet.Level)!;
        var highestMember = group.MaxBy(p => p.StatSheet.Level)!;

        // Full deduction if the level difference exceeds 19
        return Math.Abs(highestMember.StatSheet.Level - lowestMember.StatSheet.Level) > 19 ? 1.0m : 0.0m;
    }
}