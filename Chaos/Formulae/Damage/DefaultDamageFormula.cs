using System.Collections.Immutable;
using Chaos.Common.Definitions;
using Chaos.Formulae.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.SkillScripts.Abstractions;
using Chaos.Scripting.SpellScripts.Abstractions;
using Chaos.Services.Servers.Options;

// ReSharper disable UseCollectionExpression

namespace Chaos.Formulae.Damage;

public class DefaultDamageFormula : IDamageFormula
{
    protected virtual ImmutableArray<ImmutableArray<decimal>> ElementalModifierLookup { get; } = new[]
    {
        // Element Table
       // @formatter:off
        //mostly lifted from http://da-wizard.com/elements.html
        //                                                  D E F E N S E
        //                         None,  Fire,  Water,  Wind,  Earth, Holy,  Darkness,  Wood,  Metal,  Undead
        /*      None*/     new[] { 0.75m, 0.66m, 0.66m,  0.66m, 0.66m, 0.93m, 0.43m,     0.37m, 0.37m,  0.37m }.ToImmutableArray(),
        /* O    Fire*/     new[] { 1.50m, 0.75m, 0.83m,  1.33m, 1.00m, 0.93m, 0.87m,     2.01m, 0.83m,  2.01m }.ToImmutableArray(),
        /* F    Water*/    new[] { 1.50m, 1.33m, 0.75m,  1.00m, 0.83m, 0.93m, 0.87m,     0.58m, 1.74m,  0.93m }.ToImmutableArray(), 
        /* F    Wind*/     new[] { 1.50m, 0.83m, 1.00m,  0.75m, 1.33m, 0.93m, 0.87m,     1.03m, 1.74m,  0.83m }.ToImmutableArray(),
        /* E    Earth*/    new[] { 1.50m, 1.00m, 1.33m,  0.83m, 0.75m, 0.93m, 0.87m,     0.58m, 0.83m,  0.58m }.ToImmutableArray(),
        /* N    Holy*/     new[] { 2.32m, 0.76m, 0.76m,  0.76m, 0.76m, 0.58m, 1.48m,     0.58m, 0.76m,  2.01m }.ToImmutableArray(),
        /* S    Darkness*/ new[] { 2.32m, 1.15m, 1.15m,  1.15m, 1.15m, 1.48m, 0.58m,     1.48m, 0.58m,  0.58m }.ToImmutableArray(),
        /* E    Wood*/     new[] { 2.32m, 0.58m, 0.76m,  1.03m, 1.74m, 0.93m, 0.83m,     0.58m, 1.03m,  0.83m }.ToImmutableArray(),
        /*      Metal*/    new[] { 2.32m, 0.83m, 0.50m,  1.88m, 0.83m, 0.93m, 0.83m,     1.25m, 0.58m,  1.03m }.ToImmutableArray(),
        /*      Undead*/   new[] { 2.32m, 0.50m, 0.83m,  0.83m, 1.88m, 0.93m, 0.83m,     0.58m, 0.76m,  0.58m }.ToImmutableArray() 
        // @formatter:on
    }.ToImmutableArray();
    
    /// <inheritdoc />
    public int Calculate(
        Creature source,
        Creature target,
        IScript script,
        int damage,
        Element? elementOverride = null)
    {
        ApplySkillSpellModifier(ref damage, script, source);

        var defenderAc = GetDefenderAc(target);

        ApplyAcModifier(ref damage, defenderAc);
        ApplyElementalModifier(
            damage: ref damage, 
            attackElement: elementOverride ?? source.StatSheet.OffenseElement, 
            defenseElement: target.StatSheet.DefenseElement, 
            multiplier: target.StatSheet.EffectiveElementalMultiplier);

        return damage;
    }

    protected virtual void ApplyAcModifier(ref int damage, int defenderAc) => damage = Convert.ToInt32(damage * (1 + defenderAc / 100m));

    protected virtual void ApplyElementalModifier(ref int damage, Element attackElement, Element defenseElement)
        => damage = Convert.ToInt32(damage * ElementalModifierLookup[(int)attackElement][(int)defenseElement]);

    protected virtual void ApplyElementalModifier(ref int damage, Element attackElement, Element defenseElement, int multiplier)
    {
        var baseMultiplier = ElementalModifierLookup[(int)attackElement][(int)defenseElement];
        var percentageMultiplier = multiplier / 100m;
        if (multiplier > 0)
        {
            var isStrong = baseMultiplier > 1.0m;
            baseMultiplier = isStrong 
                ? baseMultiplier + percentageMultiplier 
                : baseMultiplier - percentageMultiplier;
            baseMultiplier = Math.Max(baseMultiplier, 0m); // Prevent negative multipliers
        }
        damage = Convert.ToInt32(damage * baseMultiplier);
    }
    
    protected virtual void ApplySkillSpellModifier(ref int damage, IScript source, Creature attacker)
    {
        switch (source)
        {
            case ISkillScript:
            {
                var addedFromPct = damage * (attacker.StatSheet.EffectiveSkillDamagePct / 100m);
                damage += Convert.ToInt32(attacker.StatSheet.EffectiveFlatSkillDamage + addedFromPct);

                break;
            }
            case ISpellScript:
            {
                var addedFromPct = damage * (attacker.StatSheet.EffectiveSpellDamagePct / 100m);
                damage += Convert.ToInt32(attacker.StatSheet.EffectiveFlatSpellDamage + addedFromPct);

                break;
            }
        }
    }

    protected virtual int GetDefenderAc(Creature defender)
        => defender switch
        {
            Aisling aisling => Math.Clamp(
                aisling.UserStatSheet.EffectiveAc,
                WorldOptions.Instance.MinimumAislingAc,
                WorldOptions.Instance.MaximumAislingAc),
            _ => Math.Clamp(defender.StatSheet.EffectiveAc, WorldOptions.Instance.MinimumMonsterAc, WorldOptions.Instance.MaximumMonsterAc)
        };
}