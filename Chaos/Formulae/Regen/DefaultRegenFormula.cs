using Chaos.Common.Utilities;
using Chaos.Formulae.Abstractions;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;

namespace Chaos.Formulae.Regen;

public sealed class DefaultRegenFormula : IRegenFormula
{
    private const decimal BaseMpRegenPercent = 5;
    private const decimal BaseHpRegenPercent = 10;
    private const decimal MaxRegenPercent = 20;
    private const int BaseInterval = 18;
    // Achievable regen from items should not exceed 28 so that the min interval is only possible with bard songs
    private const int MinInterval = 5;
    private const int MaxRegenStat = 30;
    
    /// <inheritdoc />
    public int CalculateHealthRegen(Creature creature)
    {
        if (creature.StatSheet.HealthPercent == 100)
            return 0;

        var percentToRegenerate = creature switch
        {
            Aisling aisling => CalculateRegenPercentage(aisling.StatSheet.Con, BaseHpRegenPercent),
            Monster  => 3,
            Merchant => 100,
            _        => throw new ArgumentOutOfRangeException(nameof(creature), creature, null)
        };

        return MathEx.GetPercentOf<int>((int)creature.StatSheet.EffectiveMaximumHp, percentToRegenerate);
    }

    public int CalculateIntervalSecs(Creature creature)
    {
        // On char creation this gets called before the statsheet is created (I think)
        if (creature.StatSheet == null) return BaseInterval; 
        if (creature.StatSheet.EffectiveRegen <= 0) return BaseInterval;
        
        var regenFactor = Math.Pow(creature.StatSheet.EffectiveRegen / (double)MaxRegenStat, 0.6);
        var interval = (int)Math.Round(BaseInterval - (regenFactor * (BaseInterval - MinInterval)));
    
        return Math.Max(interval, MinInterval);
    }

    /// <inheritdoc />
    public int CalculateManaRegen(Creature creature)
    {
        if (creature.StatSheet.ManaPercent == 100)
            return 0;

        var percentToRegenerate = creature switch
        {
            Aisling aisling => CalculateRegenPercentage(aisling.StatSheet.Wis, BaseMpRegenPercent),
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
        
        // Clamp the con stat between base and max
        var clampedStat = Math.Clamp(stat, baseStat, maxStat);

        // Calculate the percentage based on the con stat
        var regenPercent = baseRegenPercent + (clampedStat - baseStat) * (MaxRegenPercent - baseRegenPercent) / (maxStat - baseStat);

        return regenPercent;
    }
}