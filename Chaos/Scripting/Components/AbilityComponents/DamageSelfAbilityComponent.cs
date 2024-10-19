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

public struct DamageSelfAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IDamageSelfComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            var damage = CalculateDamage(
                context.Source,
                target,
                options.BaseDamage,
                options.PctHpDamage
            );

            if (damage <= 0) continue;
            if (damage > context.Source.StatSheet.CurrentHp)
            {
                switch (target)
                {
                    case Aisling aisling:
                        aisling.StatSheet.SetHp(1);
                        aisling.Client.SendAttributes(StatUpdateType.Vitality);
                        aisling.ShowHealth();
                        break;
                    case Monster _:
                        break;
                }
            }
            else
            {
                options.ApplyDamageScript.ApplyDamage(
                    context.Source,
                    target,
                    options.SourceScript,
                    damage);
            }
            
        }
    }

    private int CalculateDamage(Creature source,
        Creature target,
        int? baseDamage = null,
        decimal? pctHpDamage = null
    )
    {
        var finalDamage = baseDamage ?? 0;
        finalDamage += MathEx.GetPercentOf<int>((int)target.StatSheet.EffectiveMaximumHp, pctHpDamage ?? 0);
        return finalDamage;
    }


    public interface IDamageSelfComponentOptions
    {
        IApplyDamageScript ApplyDamageScript { get; init; }
        int? BaseDamage { get; init; }
        Stat? DamageStat { get; init; }
        decimal? DamageStatMultiplier { get; init; }
        decimal? PctHpDamage { get; init; }
        IScript SourceScript { get; init; }
    }
}
