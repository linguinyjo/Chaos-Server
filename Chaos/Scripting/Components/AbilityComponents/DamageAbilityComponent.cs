using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct DamageAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IDamageComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            var damage = CalculateDamage(
                context.Source,
                target,
                options.BaseDamage,
                options.PctHpDamage,
                options.DamageStat,
                options.DamageStatMultiplier,
                options.UsePAtk,
                options.UseMatk,
                options.FistBonus
                );

            if (damage <= 0) continue;

            var isAsleep = target.IsAsleep(out var sleepEffect);
            
            if (isAsleep)
            {
                damage *= 2;
            }
            
            options.ApplyDamageScript.ApplyDamage(
                context.Source,
                target,
                options.SourceScript,
                damage,
                options.Element);

            if (sleepEffect != null) target.Effects.Dispel(sleepEffect);
        }
    }

    private int CalculateDamage(Creature source,
        Creature target,
        int? baseDamage = null,
        decimal? pctHpDamage = null,
        Stat? damageStat = null,
        decimal? damageStatMultiplier = null,
        bool? usePAtk = null,
        bool? useMAtk = null, 
        int? fistBonus = null)
    {
        var finalDamage = baseDamage ?? 0;
        finalDamage += MathEx.GetPercentOf<int>(target.StatSheet.CurrentHp, pctHpDamage ?? 0);
        
        if (!damageStat.HasValue) return finalDamage;

        // Apply weapon damage
        if (usePAtk == true)
        {
            finalDamage += source.StatSheet.EffectivePhysicalAttack;
        }
        
        // Apply magic damage
        if (useMAtk == true)
        {
            finalDamage += source.StatSheet.EffectiveMagicAttack;
        }

        if (source is Aisling aisling && fistBonus != null && aisling.Equipment[EquipmentSlot.Weapon] == null) 
        {
            finalDamage += fistBonus.Value;
        }
        
        if (!damageStatMultiplier.HasValue)
        {
            finalDamage += source.StatSheet.GetEffectiveStat(damageStat.Value);
            return finalDamage;
        }

        finalDamage += Convert.ToInt32(source.StatSheet.GetEffectiveStat(damageStat.Value) * damageStatMultiplier.Value);

        if (source.StatSheet.DmgMod == 0) return finalDamage;
        var dmgMultiplier = 1 + (source.StatSheet.DmgMod / 100.0);
        finalDamage = Convert.ToInt32(finalDamage * dmgMultiplier);

        return finalDamage;
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
        bool? UsePAtk { get; init; }
        bool? UseMatk { get; init; }
        int? FistBonus { get; init; }
    }
}
