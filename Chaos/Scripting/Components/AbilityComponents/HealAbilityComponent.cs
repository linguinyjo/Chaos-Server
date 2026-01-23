#region
using Chaos.Common.Utilities;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;
#endregion

namespace Chaos.Scripting.Components.AbilityComponents;

public struct HealAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IHealComponentOptions>();
        var targets = vars.GetTargets<Creature>();
        var sourceScript = vars.GetSourceScript();

        var abilityMultiplier = CalculateAbilityHealMultiplier(context.SourceAisling, options.AbilityTemplateKey);
        foreach (var target in targets)
        {
            var heal = CalculateHeal(
                context.Source,
                target,
                options.BaseHeal,
                options.PctHpHeal,
                options.HealStat,
                options.HealStatMultiplier,
                options.MagicAttackMultiplier,
                abilityMultiplier);

            if (heal <= 0)
                continue;

            options.ApplyHealScript.ApplyHeal(
                context.Source,
                target,
                sourceScript,
                heal);
        }
    }

    private int CalculateHeal(Creature source,
        Creature target,
        int? baseHeal = null,
        decimal? pctHpHeal = null,
        Stat? healStat = null,
        decimal? healStatMultiplier = null,
        decimal? magicAttackMultiplier = null,
        decimal? abilityMultiplier = null
        )
    {
        var finalHeal = baseHeal ?? 0;
        finalHeal += MathEx.GetPercentOf<int>((int)target.StatSheet.EffectiveMaximumHp, pctHpHeal ?? 0);

        // Apply bonus heal from the skill level
        if (abilityMultiplier is > 0)
        {
            finalHeal = Convert.ToInt32(finalHeal * abilityMultiplier.Value);
        }
        
        if (magicAttackMultiplier.HasValue)
        {
            finalHeal += Convert.ToInt32(source.StatSheet.EffectiveMagicAttack * magicAttackMultiplier.Value); 
        }
        
        if (!healStat.HasValue) return finalHeal;
        
        if (!healStatMultiplier.HasValue)
        {
            finalHeal += source.StatSheet.GetEffectiveStat(healStat.Value);
            return finalHeal;
        }
        
        finalHeal += Convert.ToInt32(source.StatSheet.GetEffectiveStat(healStat.Value) * healStatMultiplier.Value);

        return finalHeal;
    }
    
    private static decimal CalculateAbilityHealMultiplier(Aisling? aisling, string? abilityTemplateKey)
    {
        if (aisling == null || abilityTemplateKey == null) return 0;  
        var level = aisling.SpellBook.TryGetObjectByTemplateKey(abilityTemplateKey, out var spell) 
            ? spell.Level : (byte)0;
        return (decimal)(1.0f + (level / 100f) * 0.2f);
    }

    public interface IHealComponentOptions
    {
        IApplyHealScript ApplyHealScript { get; init; }
        int? BaseHeal { get; init; }
        Stat? HealStat { get; init; }
        decimal? HealStatMultiplier { get; init; }
        decimal? MagicAttackMultiplier { get; init; }
        decimal? PctHpHeal { get; init; }
        string? AbilityTemplateKey { get; init; }
    }
}