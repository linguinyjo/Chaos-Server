using Chaos.Models.Data;
using Chaos.Schemas.Aisling;
using Chaos.Schemas.Data;
using Chaos.TypeMapper.Abstractions;

namespace Chaos.Services.MapperProfiles;

public sealed class AttributesMapperProfile : IMapperProfile<Attributes, AttributesSchema>,
                                              IMapperProfile<Stats, StatsSchema>,
                                              IMapperProfile<StatSheet, StatSheetSchema>,
                                              IMapperProfile<UserStatSheet, UserStatSheetSchema>
{
    public Attributes Map(AttributesSchema obj)
        => new()
        {
            Ac = obj.Ac,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Str = obj.Str,
            Int = obj.Int,
            Wis = obj.Wis,
            Con = obj.Con,
            Dex = obj.Dex,
            MagicResistance = obj.MagicResistance,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSpellDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };

    public AttributesSchema Map(Attributes obj)
        => new()
        {
            Ac = obj.Ac,
            Con = obj.Con,
            Dex = obj.Dex,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Int = obj.Int,
            MagicResistance = obj.MagicResistance,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            Str = obj.Str,
            Wis = obj.Wis,
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSkillDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };

    /// <inheritdoc />
    public Stats Map(StatsSchema obj)
        => new()
        {
            Str = obj.Str,
            Int = obj.Int,
            Wis = obj.Wis,
            Con = obj.Con,
            Dex = obj.Dex
        };

    /// <inheritdoc />
    public StatsSchema Map(Stats obj)
        => new()
        {
            Str = obj.Str,
            Int = obj.Int,
            Wis = obj.Wis,
            Con = obj.Con,
            Dex = obj.Dex
        };

    public StatSheet Map(StatSheetSchema obj)
        => new()
        {
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSpellDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            Ac = obj.Ac,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Str = obj.Str,
            Int = obj.Int,
            Wis = obj.Wis,
            Con = obj.Con,
            Dex = obj.Dex,
            MagicResistance = obj.MagicResistance,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            CurrentHp = obj.CurrentHp,
            CurrentMp = obj.CurrentMp,
            AbilityLevel = obj.AbilityLevel,
            Level = obj.Level,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };

    public StatSheetSchema Map(StatSheet obj)
        => new()
        {
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSpellDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            AbilityLevel = obj.AbilityLevel,
            Ac = obj.Ac,
            Con = obj.Con,
            CurrentHp = obj.CurrentHp,
            CurrentMp = obj.CurrentMp,
            Level = obj.Level,
            Dex = obj.Dex,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Int = obj.Int,
            MagicResistance = obj.MagicResistance,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            Str = obj.Str,
            Wis = obj.Wis,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };

    public UserStatSheet Map(UserStatSheetSchema obj)
        => new()
        {
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSpellDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            Ac = obj.Ac,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Str = obj.Str,
            Int = obj.Int,
            Wis = obj.Wis,
            Con = obj.Con,
            Dex = obj.Dex,
            MagicResistance = obj.MagicResistance,
            Master = obj.Master,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            CurrentHp = obj.CurrentHp,
            CurrentMp = obj.CurrentMp,
            AbilityLevel = obj.AbilityLevel,
            Level = obj.Level,
            ToNextAbility = obj.ToNextAbility,
            ToNextLevel = obj.ToNextLevel,
            TotalAbility = obj.TotalAbility,
            TotalExp = obj.TotalExp,
            UnspentPoints = obj.UnspentPoints,
            BaseClass = obj.BaseClass,
            AdvClass = obj.AdvClass,
            MaxWeight = obj.MaxWeight,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };

    public UserStatSheetSchema Map(UserStatSheet obj)
        => new()
        {
            AtkSpeedPct = obj.AtkSpeedPct,
            FlatSkillDamage = obj.FlatSkillDamage,
            FlatSpellDamage = obj.FlatSpellDamage,
            SkillDamagePct = obj.SkillDamagePct,
            SpellDamagePct = obj.SpellDamagePct,
            AbilityLevel = obj.AbilityLevel,
            Ac = obj.Ac,
            AdvClass = obj.AdvClass,
            BaseClass = obj.BaseClass,
            Con = obj.Con,
            CurrentHp = obj.CurrentHp,
            CurrentMp = obj.CurrentMp,
            Level = obj.Level,
            ToNextAbility = obj.ToNextAbility,
            ToNextLevel = obj.ToNextLevel,
            TotalAbility = obj.TotalAbility,
            TotalExp = obj.TotalExp,
            UnspentPoints = obj.UnspentPoints,
            Dex = obj.Dex,
            Dmg = obj.Dmg,
            Hit = obj.Hit,
            Int = obj.Int,
            Master = obj.Master,
            MagicResistance = obj.MagicResistance,
            MaximumHp = obj.MaximumHp,
            MaximumMp = obj.MaximumMp,
            Str = obj.Str,
            Wis = obj.Wis,
            MaxWeight = obj.MaxWeight,
            PhysicalAttack = obj.PhysicalAttack,
            MagicAttack = obj.MagicAttack,
        };
}