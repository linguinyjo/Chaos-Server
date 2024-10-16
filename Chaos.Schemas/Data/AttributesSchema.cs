#pragma warning disable CS1591
namespace Chaos.Schemas.Data;

/// <summary>
///     Represents the serializable schema of an object's attributes
/// </summary>
public record AttributesSchema : StatsSchema
{
    public int Ac { get; set; }
    public int AtkSpeedPct { get; set; }
    public int Dmg { get; set; }
    public int FlatSkillDamage { get; set; }
    public int FlatSpellDamage { get; set; }
    public int Hit { get; set; }
    public int MagicResistance { get; set; }
    public int MaximumHp { get; set; }
    public int MaximumMp { get; set; }
    public int SkillDamagePct { get; set; }
    public int SpellDamagePct { get; set; }
    public int PhysicalAttack { get; set; }
    public int MagicAttack { get; set; }
    public int Regen { get; set; }
}

public record struct SpellCastTimeReduction
{
    public int Heal { get; set; }
    public int Debuff { get; set; }
    public int Buff { get; set; }
    public int Cc { get; set; }
    public int Cure { get; set; }
}