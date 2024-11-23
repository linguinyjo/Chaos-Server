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
    private const int MinInterval = 5;
    private const int MaxRegenStat = 30;
   
    private const int BaseStat = 3;
    private const int MaxStat = 110;
    
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

    // Achievable regen from items should not exceed 28 so that the min interval is only possible with bard songs
    // Current breakpoints:
    // 0:  18
    // 1:  16
    // 2:  15
    // 4:  14
    // 6:  13
    // 8:  12
    // 10:  11
    // 12:  10
    // 15:  9
    // 18:  8
    // 22:  7
    // 25:  6
    // 29:  5
    // 30:  5
    public int CalculateIntervalSecs(Creature creature)
    {
        // On char creation this gets called before the statsheet is created (I think)
        if (creature?.StatSheet == null) return BaseInterval; 
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
        var clampedStat = Math.Clamp(stat, BaseStat, MaxStat);
        return baseRegenPercent + (clampedStat - BaseStat) * (MaxRegenPercent - baseRegenPercent) / (MaxStat - BaseStat);
    }
}