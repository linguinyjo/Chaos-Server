using Chaos.Formulae.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.World;

namespace Chaos.Formulae.LevelUp;

public class DefaultLevelUpFormula : ILevelUpFormula
{
    /// <inheritdoc />
    public virtual Attributes CalculateAttributesIncrease(Aisling aisling)
    {
        var conMod = (double)aisling.StatSheet.Con / aisling.StatSheet.Level + 1;
        var wisMod = (double)aisling.StatSheet.Wis / aisling.StatSheet.Level + 1;

        var random = new Random();
        var hpRandom = random.Next(21, 41);  
        var mpRandom = random.Next(11, 21);  
        
        var (maxHpGain, maxMpGain) = GetMaxHpMpGain(aisling.StatSheet.Level);

        var hpGain = Math.Min(Math.Round((conMod * 50.0 + hpRandom), 2), maxHpGain);
        var mpGain = Math.Min(Math.Round((wisMod * 25.0 + mpRandom), 2), maxMpGain);
        
        return new Attributes
        {
            MaximumHp = Convert.ToInt32(hpGain),
            MaximumMp = Convert.ToInt32(mpGain),
            Ac = (aisling.StatSheet.Level % 3) == 0 ? -1 : 0
        };
    }

    /// <inheritdoc />
    public virtual int CalculateMaxWeight(Aisling aisling) => 40 + aisling.UserStatSheet.Level / 2 + aisling.UserStatSheet.Str;

    /// <inheritdoc />
    public virtual int CalculateTnl(Aisling aisling)
    {
        var level = aisling.UserStatSheet.Level;

        double divisionFactor = level switch
        {
            >= 1 and <= 21 => 6,
            >= 22 and <= 41 => 5,
            >= 42 and <= 61 => 4,
            >= 62 and <= 79 => 3,
            >= 80 and <= 99 => 2,
            _ => 6
        };
        return Convert.ToInt32((Math.Pow(level, 2) * 200) / divisionFactor);    
    } 
    
    private static (int maxHpGain, int maxMpGain) GetMaxHpMpGain(int level)
    {
        return level >= 5 ? (175, 99) : (97, 75);
    }
}