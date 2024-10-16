using Chaos.Formulae.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.World;

namespace Chaos.Formulae.LevelUp;

public class DefaultLevelUpFormula : ILevelUpFormula
{
    /// <inheritdoc />
    public virtual Attributes CalculateAttributesIncrease(Aisling aisling)
    {
        var conMod = (double)aisling.StatSheet.Con / (aisling.StatSheet.Level + 1);
        var wisMod = (double)aisling.StatSheet.Wis / (aisling.StatSheet.Level + 1);

        var random = new Random();
        var hpRandom = random.Next(21, 41);  
        var mpRandom = random.Next(11, 21);  
        
        var (maxHpGain, maxMpGain) = GetMaxHpMpGain(aisling.StatSheet.Level);

        var hpGain = Math.Min(Math.Round((conMod * 50.0 + hpRandom), 2), maxHpGain);
        var mpGain = Math.Min(Math.Round((wisMod * 25.0 + mpRandom), 2), maxMpGain);
        
        return new Attributes
        {
            //each level, add (Level * 0.3) + 10 hp
            MaximumHp = Convert.ToInt32(hpGain),

            //each level, add (Level * 0.15) + 5 mp
            MaximumMp = Convert.ToInt32(mpGain),

            //every 3 levels, subtract 1 ac
            Ac = (aisling.StatSheet.Level % 3) == 0 ? -1 : 0
        };
    }

    /// <inheritdoc />
    public virtual int CalculateMaxWeight(Aisling aisling) => 40 + aisling.UserStatSheet.Level / 2 + aisling.UserStatSheet.Str;

    /// <inheritdoc />
    public virtual int CalculateTnl(Aisling aisling) => Convert.ToInt32((Math.Pow(aisling.UserStatSheet.Level, 2) * 200) / 4);
    
    private static (int maxHpGain, int maxMpGain) GetMaxHpMpGain(int level)
    {
        if (level >= 11) return (200, 100);
        return level >= 6 ? (100, 75) : (75, 50);
    }
}