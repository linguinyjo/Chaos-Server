using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions.Geometry;
using Chaos.Geometry.Abstractions.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Scripting.MonsterScripts;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Microsoft.Extensions.Options;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct DamageAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IDamageComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        var abilityDamageMultiplier = CalculateAbilityDamageMultiplier(
            context.SourceAisling, 
            options.AbilityTemplateKey, 
            options.IsSpell);
        
        foreach (var target in targets)
        {
            var damage = CalculateDamage(context.Source, target, options, abilityDamageMultiplier);

            if (damage <= 0) continue;

            var isAsleep = target.IsAsleep(out var sleepEffect);
            if (isAsleep) damage *= 2;
            
            options.ApplyDamageScript.ApplyDamage(
                context.Source,
                target,
                options.SourceScript,
                damage,
                options.Element);
            
            // TODO this probably belongs in its own functional script but i am too lazy to write one for now
            ApplyDurabilityLoss(target);
            if (sleepEffect != null) target.Effects.Dispel(sleepEffect);
        }
    }
    
    /// <summary>
    /// Calculates the total damage for an attack
    /// </summary>
    private static int CalculateDamage(
        Creature source, 
        Creature target, 
        IDamageComponentOptions components, 
        decimal abilityDamageMultiplier)
    {
        var finalDamage = components.BaseDamage ?? 0;
        finalDamage = CalculateHpDamage(source, finalDamage, components);
        finalDamage = ApplyStatDamage(source, finalDamage, components);
        finalDamage = ApplyWeaponDamage(source, finalDamage, components);
        finalDamage = ApplyAbilityMultiplier(finalDamage, abilityDamageMultiplier);
        finalDamage = ApplyDamageModifier(source, finalDamage);
        finalDamage = ApplyDirectionalDamage(source, target, components, finalDamage);

        return finalDamage;
    }
    
    /// <summary>
    /// Calculates base damage including percentage HP damage
    /// </summary>
    private static int CalculateHpDamage(Creature source, int currentDamage, IDamageComponentOptions components)
    {
        if (!components.PctHpDamage.HasValue) return currentDamage;
        if (components.PctHpDamage.HasValue)
        {
            currentDamage += MathEx.GetPercentOf<int>(
                source.StatSheet.CurrentHp, 
                (decimal)components.PctHpDamage);
        }
        return currentDamage;
    }

    /// <summary>
    /// Applies stat-based damage if a damage stat is specified
    /// </summary>
    private static int ApplyStatDamage(Creature source, int currentDamage, IDamageComponentOptions components)
    {
        if (!components.DamageStat.HasValue) return currentDamage;

        var statValue = source.StatSheet.GetEffectiveStat(components.DamageStat.Value);
        
        return components.DamageStatMultiplier.HasValue
            ? currentDamage + Convert.ToInt32(statValue * components.DamageStatMultiplier.Value)
            : currentDamage + statValue;
    }

    /// <summary>
    /// Applies damage multiplier from the abilities level
    /// </summary>
    private static int ApplyAbilityMultiplier(int currentDamage, decimal abilityDamageMultiplier)
    {
        return abilityDamageMultiplier > 0
            ? Convert.ToInt32(currentDamage * abilityDamageMultiplier)
            : currentDamage;
    }

    /// <summary>
    /// Applies weapon and magic attack damage
    /// </summary>
    private static int ApplyWeaponDamage(Creature source, int currentDamage, IDamageComponentOptions components)
    {
        decimal statMultiplier = 1;
        if (components.DamageStat != null)
        {
            var stat = source.StatSheet.GetEffectiveStat(components.DamageStat.Value);
            statMultiplier = 1 + (stat * 0.005m);
        }

        // Apply physical attack bonus
        if (components.PAtkMultiplier.HasValue)
        {
            var multiplier = 1 + (components.PAtkMultiplier.Value / 100);
            var weaponDamageBonus = Convert.ToInt32(source.StatSheet.EffectivePhysicalAttack * multiplier );
           
            currentDamage += Convert.ToInt32(weaponDamageBonus * statMultiplier);
            
        }

        // Apply magic attack if specified
        if (components.UseMatk.HasValue)
        {
            currentDamage += Convert.ToInt32(source.StatSheet.EffectiveMagicAttack * statMultiplier);
        }

        // Apply fist bonus for unarmed Aisling
        if (source is Aisling aisling && 
            components.FistBonus.HasValue && 
            aisling.Equipment[EquipmentSlot.Weapon] == null)
        {
            currentDamage += Convert.ToInt32( components.FistBonus.Value * statMultiplier);
        }

        return currentDamage;
    }

    /// <summary>
    /// Applies damage modifier from Dmg stat
    /// </summary>
    private static int ApplyDamageModifier(Creature source, int currentDamage)
    {
        if (source.StatSheet.DmgMod == 0) return currentDamage;
        
        var dmgMultiplier = 1 + (source.StatSheet.DmgMod / 100.0);
        return Convert.ToInt32(currentDamage * dmgMultiplier);
    }

    // From behind == 1.5x and from the side == 1.25x
    private static int ApplyDirectionalDamage(Creature source, Creature target, IDamageComponentOptions components, int finalDamage)
    {
        if (components.IsSpell is true) return finalDamage;
        if (source.Direction == target.Direction)
        {
            return Convert.ToInt32(finalDamage * 1.5);
        }
        var (side1, side2) = target.Direction.GetSideDirections();
        if (source.Direction == side1 || source.Direction == side2)
        {
            return Convert.ToInt32(finalDamage * 1.25);
        }
        return finalDamage;
    }
    
    private static decimal CalculateAbilityDamageMultiplier(Aisling? aisling, string? abilityTemplateKey, bool? isSpell)
    {
        if (aisling == null || abilityTemplateKey == null) return 0;  
        var level = GetAbilityLevel(aisling, abilityTemplateKey, isSpell);
        return (decimal)(1.0f + (level / 100f) * 0.2f);
    }
    
    private static byte GetAbilityLevel(Aisling aisling, string abilityTemplateKey, bool? isSpell)
    {
        if (isSpell == true) 
        {
            return aisling.SpellBook.TryGetObjectByTemplateKey(abilityTemplateKey, out var spell) 
                ? spell.Level : (byte)0;
        }
        return aisling.SkillBook.TryGetObjectByTemplateKey(abilityTemplateKey, out var skill) 
            ? skill.Level : (byte)0;
    }


    private static void ApplyDurabilityLoss(Creature target)
    {
        if (target is not Aisling aisling) return;
        foreach (var item in aisling.Equipment)
        {
            if (item.Template.MaxDurability == null) continue;

            item.CurrentDurability -= 1;
            if (item.CurrentDurability <= 0)
            {
                aisling.Equipment.Remove(item.Slot);
                aisling.SendOrangeBarMessage($"Your {item.Template.Name} has been destroyed");
                continue;
            }
            var currentPercentage = (item.CurrentDurability / (float)item.Template.MaxDurability) * 100;

            if (!(currentPercentage <= 10) || item.HasShownLowDurabilityWarning) continue;
            item.HasShownLowDurabilityWarning = true;
            aisling.SendOrangeBarMessage($"Warning: {item.Template.Name} has less than 10% durability remaining!");
        }
    }
    
    public interface IDamageComponentOptions
    {
        IApplyDamageScript ApplyDamageScript { get; init; }
        int? BaseDamage { get; init; }
        Stat? DamageStat { get; init; }
        decimal? DamageStatMultiplier { get; init; }
        Element? Element { get; init; }
        decimal? PctHpDamage { get; init; }
        IScript SourceScript { get; init; }
        decimal? PAtkMultiplier { get; init; }
        bool? UseMatk { get; init; }
        int? FistBonus { get; init; }
        string? AbilityTemplateKey { get; init; }
        bool? IsSpell { get; init; }
    }
}
