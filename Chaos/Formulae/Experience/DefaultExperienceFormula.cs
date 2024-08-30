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
            2 => 0,
            3 => 0.20m,
            4 => 0.30m,
            5 => 0.40m,
            6 => 0.50m,
            _ => throw new ArgumentOutOfRangeException(nameof(group.Count), "Group size is too large.")
        };

    // ReSharper disable once ParameterTypeCanBeEnumerable.Global
    protected virtual decimal GetMonsterLevelDifferenceDeductions(ICollection<Aisling> group, Monster monster)
    {
        var highestPlayerLevel = group.Max(p => p.StatSheet.Level);
        var monsterLevel = monster.StatSheet.Level;
        var levelDifference = highestPlayerLevel - monsterLevel;

        switch (levelDifference)
        {
            case <= 4:
            {
                // Use existing logic for level differences of 4 or less
                var upperBound = LevelRangeFormulae.Default.GetUpperBound(highestPlayerLevel);
                var lowerBound = LevelRangeFormulae.Default.GetLowerBound(highestPlayerLevel);

                if ((monsterLevel >= lowerBound) && (monsterLevel <= upperBound))
                    return 0;

                var bounds = monsterLevel < highestPlayerLevel ? lowerBound : upperBound;
                var stepSize = Math.Abs(bounds - highestPlayerLevel) / 2.0m;
                var faultSize = Math.Abs(bounds - monsterLevel);

                return Math.Min(1, faultSize / stepSize * 0.25m);
            }
            case 5:
                // 25% reduction for 5 levels higher
                return 0.25m;
            case 6:
                // 50% reduction for 6 levels higher
                return 0.50m;
            default:
                // For 7 or more levels difference, we'll return a special value
                // to indicate that the exp should be set to 1
                return decimal.MaxValue;
        }
    }

    protected virtual decimal GetPartyLevelDifferenceDeductions(ICollection<Aisling> group)
    {
        var lowestMember = group.MinBy(p => p.StatSheet.Level)!;
        var highestMember = group.MaxBy(p => p.StatSheet.Level)!;

        if (lowestMember.WithinLevelRange(highestMember))
            return 0;

        var lowerBound = LevelRangeFormulae.Default.GetLowerBound(highestMember.StatSheet.Level);
        var stepSize = (highestMember.StatSheet.Level - lowerBound) / 2.0m;
        var faultSize = lowerBound - lowestMember.StatSheet.Level;

        return Math.Min(1, faultSize / stepSize * 0.25m);
    }
}