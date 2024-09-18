using Chaos.Common.Utilities;
using Chaos.Formulae.Abstractions;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;

namespace Chaos.Formulae.Regen;

public sealed class DefaultRegenFormula : IRegenFormula
{
    
    const decimal baseMpRegenPercent = 5;
    const decimal baseHpRegenPercent = 10;

    /// <inheritdoc />
    public int CalculateHealthRegen(Creature creature)
    {
        if (creature.StatSheet.HealthPercent == 100)
            return 0;

        var percentToRegenerate = creature switch
        {
            Aisling aisling => CalculateRegenPercentage(aisling.StatSheet.Con, baseHpRegenPercent),
            Monster  => 3,
            Merchant => 100,
            _        => throw new ArgumentOutOfRangeException(nameof(creature), creature, null)
        };

        return MathEx.GetPercentOf<int>((int)creature.StatSheet.EffectiveMaximumHp, percentToRegenerate);
    }

    /// <inheritdoc />
    public int CalculateIntervalSecs(Creature creature)
    {
        const int baseInterval = 20;
        const int minInterval = 8;
        const int maxRegenStat = 15;
        // On char creation this gets called before the statsheet is created (I think)
        if (creature.StatSheet == null) return baseInterval; 
        if (creature.StatSheet.EffectiveRegen <= 0) return baseInterval;
        var interval = baseInterval - (creature.StatSheet.EffectiveRegen * (baseInterval - minInterval) / maxRegenStat);
        return Math.Max(interval, minInterval);
    }

    /// <inheritdoc />
    public int CalculateManaRegen(Creature creature)
    {
        if (creature.StatSheet.ManaPercent == 100)
            return 0;

        var percentToRegenerate = creature switch
        {
            Aisling aisling => CalculateRegenPercentage(aisling.StatSheet.Wis, baseMpRegenPercent),
            Monster  => 1.5m,
            Merchant => 100,
            _        => throw new ArgumentOutOfRangeException(nameof(creature), creature, null)
        };

        return MathEx.GetPercentOf<int>((int)creature.StatSheet.EffectiveMaximumMp, percentToRegenerate);
    }
    
    private static decimal CalculateRegenPercentage(int stat, decimal baseRegenPercent)
    {
        const int baseStat = 3;
        const int maxStat = 110;
        const decimal maxRegenPercent = 20;

        // Clamp the con stat between base and max
        var clampedStat = Math.Clamp(stat, baseStat, maxStat);

        // Calculate the percentage based on the con stat
        var regenPercent = baseRegenPercent + (clampedStat - baseStat) * (maxRegenPercent - baseRegenPercent) / (maxStat - baseStat);

        return regenPercent;
    }
}