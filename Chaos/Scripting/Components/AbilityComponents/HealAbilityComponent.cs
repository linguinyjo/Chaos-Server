using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct HealAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IHealComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            var heal = CalculateHeal(
                context.Source,
                target,
                options.BaseHeal,
                options.PctHpHeal,
                options.HealStat,
                options.HealStatMultiplier);

            if (heal <= 0)
                continue;

            options.ApplyHealScript.ApplyHeal(
                context.Source,
                target,
                options.SourceScript,
                heal);
        }
    }

    private int CalculateHeal(
        Creature source,
        Creature target,
        int? baseHeal = null,
        decimal? pctHpHeal = null,
        Stat? healStat = null,
        decimal? healStatMultiplier = null)
    {
        var finalHeal = baseHeal ?? 0;
        finalHeal += MathEx.GetPercentOf<int>((int)target.StatSheet.EffectiveMaximumHp, pctHpHeal ?? 0);

        if (!healStat.HasValue) return finalHeal;
        
        if (!healStatMultiplier.HasValue)
        {
            finalHeal += source.StatSheet.GetEffectiveStat(healStat.Value);
            finalHeal += source.StatSheet.EffectiveMagicAttack;
            return finalHeal;
        }
        
        finalHeal += Convert.ToInt32(source.StatSheet.EffectiveMagicAttack * healStatMultiplier.Value); ;
        finalHeal += Convert.ToInt32(source.StatSheet.GetEffectiveStat(healStat.Value) * healStatMultiplier.Value);

        return finalHeal;
    }

    public interface IHealComponentOptions
    {
        IApplyHealScript ApplyHealScript { get; init; }
        int? BaseHeal { get; init; }
        Stat? HealStat { get; init; }
        decimal? HealStatMultiplier { get; init; }
        decimal? PctHpHeal { get; init; }
        IScript SourceScript { get; init; }
    }
}