using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct AbilityLevellingAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        if (vars.GetTargets<Creature>().Count == 0) return;
        var options = vars.GetOptions<IAbilityLevellingComponentOptions>();
        if (options.AbilityTemplateKey == null) return;
        if (options.LevelUpRate == null) return;
        var aisling = context.SourceAisling;
        if (aisling == null) return;
        if (options.IsSpell == true) UpdateSpellCount(aisling, options.AbilityTemplateKey, options.LevelUpRate.Value);
        else UpdateSkillCount(aisling, options.AbilityTemplateKey, options.LevelUpRate.Value);
    }

    private static void UpdateSkillCount(Aisling aisling, string abilityTemplateKey, AbilityLevellingRate levelUpRate)
    {
        if (!aisling.SkillBook.TryGetObjectByTemplateKey(abilityTemplateKey, out var skill) ||
            skill.Level >= skill.MaxLevel) return;
        var currentUses = IncrementCounter(aisling, abilityTemplateKey);
        var requiredUses = SkillLevellingConfig.GetSkillRequiredUses(skill.Level, levelUpRate, skill.Template.IsAssail);
        if (currentUses < requiredUses) return;
        aisling.SkillBook.Update(skill.Slot, lSkill => lSkill.Level = (byte)(lSkill.Level + 1));
        ResetCounter(aisling,abilityTemplateKey);
        aisling.SendOrangeBarMessage($"{skill.Template.Name} has improved");
    }
    
    private static void UpdateSpellCount(Aisling aisling, string abilityTemplateKey, AbilityLevellingRate levelUpRate)
    {
        if (!aisling.SpellBook.TryGetObjectByTemplateKey(abilityTemplateKey, out var spell) ||
            spell.Level >= spell.MaxLevel) return;
        var currentUses = IncrementCounter(aisling, abilityTemplateKey);
        var requiredUses = SkillLevellingConfig.GetSpellRequiredUses(spell.Level, levelUpRate);
        if (currentUses < requiredUses) return;
        aisling.SpellBook.Update(spell.Slot, lSpell => lSpell.Level = (byte)(lSpell.Level + 1));
        ResetCounter(aisling, abilityTemplateKey);
        aisling.SendOrangeBarMessage($"{spell.Template.Name} has improved");
    }

    private static int IncrementCounter(Aisling aisling, string abilityTemplateKey)
    {
        aisling.Trackers.Counters.AddOrIncrement(abilityTemplateKey);
        aisling.Trackers.Counters.TryGetValue(abilityTemplateKey, out var value);
        Console.WriteLine($"Updating {abilityTemplateKey} to {value}");
        return value;
    }
    
    private static void ResetCounter(Aisling aisling, string abilityTemplateKey)
    {
        aisling.Trackers.Counters.Set(abilityTemplateKey, 0);
    }
        
    public interface IAbilityLevellingComponentOptions
    {
        AbilityLevellingRate? LevelUpRate { get; init; }
        string? AbilityTemplateKey { get; init; }
        bool? IsSpell { get; init; }
    }
}

public enum AbilityLevellingRate
{
    VerySlow = 1,
    Slow = 2,
    Medium = 3,
    Fast = 4,
    VeryFast = 5,
}

public static class SkillLevellingConfig
{
    private static readonly Dictionary<AbilityLevellingRate, (int baseUses, int incrementPerLevel)>  SkillRate = new()
    {
        // Very Fast: Total Uses: 5,850 → Total Time: 24.38 hours (1.02 days)
        { AbilityLevellingRate.VeryFast, (30, 1) },
        // Fast: Total Uses: 10,100 → Total Time: 42.08 hours (1.75 days)
        { AbilityLevellingRate.Fast, (50, 1) },
        // Medium: Total Uses: 17,450 → Total Time: 72.71 hours (3.03 days)
        { AbilityLevellingRate.Medium, (75, 2) },
        // Slow: Total Uses: 23,950 → Total Time: 99.79 hours (4.16 days)
        { AbilityLevellingRate.Slow, (125, 2) },
        // Very Slow: Total Uses: 28,450 → Total Time: 118.54 hours (4.94 days)
        { AbilityLevellingRate.VerySlow, (150, 2) }
    };
    
    private static readonly Dictionary<AbilityLevellingRate, (int baseUses, int incrementPerLevel)> AssailRate = new()
    {
        // Very Fast: Total Uses: 59,800 → Total Time: 24.92 hours (1.04 days)
        { AbilityLevellingRate.VeryFast, (800, 4) },
        // Fast: Total Uses: 91,200 → Total Time: 38 hours (1.58 days)
        { AbilityLevellingRate.Fast, (1200, 6) },
        // Medium: Total Uses: 124,600 → Total Time: 51.92 hours (2.16 days)
        { AbilityLevellingRate.Medium, (1520, 8) },
        // Slow: Total Uses: 160,100 → Total Time: 66.71 hours (2.78 days)
        { AbilityLevellingRate.Slow, (1800, 10) },
        // Very Slow: Total Uses: 197,700 → Total Time: 82.38 hours (3.43 days)
        { AbilityLevellingRate.VerySlow, (2120, 12) }
    };
    
    private static readonly Dictionary<AbilityLevellingRate, (int baseUses, int incrementPerLevel)>  SpellRate = new()
    {
        // Very Fast: 20 mins → 30 mins (800 → 1200 attacks)
        { AbilityLevellingRate.VeryFast, (100, 2) },
        // Fast: 30 mins → 45 mins (1200 → 1800 attacks)  
        { AbilityLevellingRate.Fast, (150, 3) },
        // Medium: 38 mins → 57 mins (1520 → 2280 attacks)
        { AbilityLevellingRate.Medium, (225, 3) },
        // Slow: 45 mins → 68 mins (1800 → 2720 attacks)
        { AbilityLevellingRate.Slow, (325, 4) },
        // Very Slow: 53 mins → 80 mins (2120 → 3200 attacks)
        { AbilityLevellingRate.VerySlow, (450, 5) }
    };

    public static int GetSkillRequiredUses(
        int currentLevel, 
        AbilityLevellingRate rate,
        bool isAssail
    )
    {
        var (baseUses, incrementPerLevel) = isAssail ? AssailRate[rate] : SkillRate[rate];
        return baseUses + (currentLevel * incrementPerLevel);
    }
    
    public static int GetSpellRequiredUses(
        int currentLevel, 
        AbilityLevellingRate rate
    )
    {
        var (baseUses, incrementPerLevel) = SpellRate[rate];
        return baseUses + (currentLevel * incrementPerLevel);
    }
}
