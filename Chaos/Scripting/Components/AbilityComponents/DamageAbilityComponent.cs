using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
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
            // var damage = CalculateDamage(
            //     context.Source,
            //     target,
            //     options.BaseDamage,
            //     options.PctHpDamage,
            //     options.DamageStat,
            //     options.DamageStatMultiplier,
            //     options.PAtkMultiplier,
            //     options.UseMatk,
            //     options.FistBonus,
            //     abilityDamageMultiplier
            //     );

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
        finalDamage = ApplyAbilityMultiplier(finalDamage, abilityDamageMultiplier);
        finalDamage = ApplyWeaponDamage(source, finalDamage, components);
        finalDamage = ApplyDamageModifier(source, finalDamage);
        finalDamage = ApplyBackstabDamage(source, target, finalDamage);

        return finalDamage;
    }

    //TODO need to tidy this function up, mabe seperate it out into different ability components
    // private static int CalculateDamage(
    //     Creature source,
    //     Creature target,
    //     int? baseDamage = null,
    //     decimal? pctHpDamage = null,
    //     Stat? damageStat = null,
    //     decimal? damageStatMultiplier = null,
    //     decimal? PAtkMultiplier = null,
    //     bool? useMAtk = null,
    //     int? fistBonus = null, 
    //     decimal? abilityDamageMultiplier = null)
    // {
    //     var finalDamage = baseDamage ?? 0;
    //     if (pctHpDamage.HasValue)
    //     {
    //         finalDamage += MathEx.GetPercentOf<int>(source.StatSheet.CurrentHp, (decimal)pctHpDamage);
    //     }
    //     
    //     if (!damageStat.HasValue)
    //     {
    //         if (abilityDamageMultiplier is > 0 && finalDamage > 0)
    //         {
    //             finalDamage = Convert.ToInt32(finalDamage * abilityDamageMultiplier.Value);
    //         }
    //         return finalDamage;
    //     } 
    //     
    //     if (!damageStatMultiplier.HasValue)
    //     {
    //         finalDamage += source.StatSheet.GetEffectiveStat(damageStat.Value);
    //     } else
    //     {
    //         finalDamage += Convert.ToInt32(source.StatSheet.GetEffectiveStat(damageStat.Value) * damageStatMultiplier.Value);
    //     }
    //     
    //     // Apply bonus damage from the skill here before weapon attack is added
    //     if (abilityDamageMultiplier is > 0)
    //     {
    //         finalDamage = Convert.ToInt32(finalDamage * abilityDamageMultiplier.Value);
    //     }
    //     
    //     // Apply weapon damage
    //     if (PAtkMultiplier.HasValue)
    //     {
    //         var multiplier = 1 + (PAtkMultiplier.Value / 100);
    //         var weaponDamageBonus = Convert.ToInt32(source.StatSheet.EffectivePhysicalAttack * multiplier);
    //         finalDamage += weaponDamageBonus;
    //     }
    //     
    //     // Apply magic damage
    //     if (useMAtk == true)
    //     {
    //         finalDamage += source.StatSheet.EffectiveMagicAttack;
    //     }
    //
    //     if (source is Aisling aisling && fistBonus != null && aisling.Equipment[EquipmentSlot.Weapon] == null) 
    //     {
    //         finalDamage += fistBonus.Value;
    //     }
    //     
    //     if (source.StatSheet.DmgMod == 0) return finalDamage;
    //     var dmgMultiplier = 1 + (source.StatSheet.DmgMod / 100.0);
    //     finalDamage = Convert.ToInt32(finalDamage * dmgMultiplier);
    //
    //     return finalDamage;
    // }
    
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
        // Apply physical attack bonus
        if (components.PAtkMultiplier.HasValue)
        {
            var multiplier = 1 + (components.PAtkMultiplier.Value / 100);
            var weaponDamageBonus = Convert.ToInt32(
                source.StatSheet.EffectivePhysicalAttack * multiplier);
            currentDamage += weaponDamageBonus;
        }

        // Apply magic attack if specified
        if (components.UseMatk.HasValue)
        {
            currentDamage += source.StatSheet.EffectiveMagicAttack;
        }

        // Apply fist bonus for unarmed Aisling
        if (source is Aisling aisling && 
            components.FistBonus.HasValue && 
            aisling.Equipment[EquipmentSlot.Weapon] == null)
        {
            currentDamage += components.FistBonus.Value;
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

    private static int ApplyBackstabDamage(Creature source, Creature target, int finalDamage)
    {
        if(source is not Aisling aisling) return finalDamage;
        if (source.Direction == target.Direction && aisling.UserStatSheet.AdvClass is AdvClass.Assassin)
        {
            finalDamage += Convert.ToInt32(finalDamage * 1.5);
        }
        return finalDamage;
    }
    
    private static decimal CalculateAbilityDamageMultiplier(Aisling? aisling, string abilityTemplateKey, bool? isSpell)
    {
        if (aisling == null) return 0;  
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
        string AbilityTemplateKey { get; init; }
        bool? IsSpell { get; init; }
    }
}
