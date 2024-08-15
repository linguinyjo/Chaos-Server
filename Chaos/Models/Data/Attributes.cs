// ReSharper disable InconsistentNaming

namespace Chaos.Models.Data;

public record Attributes : Stats
{
    protected int _ac;
    protected int _atkSpeedPct;
    protected int _dmg; // 1% damage increase
    protected int _flatSkillDamage;
    protected int _flatSpellDamage;
    protected int _hit; // maybe rename this to spell hit or something
    protected int _magicResistance;
    protected int _maximumHp;
    protected int _maximumMp;
    protected int _skillDamagePct;
    protected int _spellDamagePct;
    protected int _physicalAttack; // Weapon stat representing damage, a single value (do we really need weapon attack ranges?)
    protected int _magicAttack; // Weapon stat representing damage for offensive spells, and bonus to heal amount - maybe should also have an impact on ability to land spells?
    
    public int Ac
    {
        get => _ac;
        init => _ac = value;
    }

    public int AtkSpeedPct
    {
        get => _atkSpeedPct;
        init => _atkSpeedPct = value;
    }

    public int Dmg
    {
        get => _dmg;
        init => _dmg = value;
    }

    public int FlatSkillDamage
    {
        get => _flatSkillDamage;
        set => _flatSkillDamage = value;
    }

    public int FlatSpellDamage
    {
        get => _flatSpellDamage;
        set => _flatSpellDamage = value;
    }

    public int Hit
    {
        get => _hit;
        init => _hit = value;
    }

    public int MagicResistance
    {
        get => _magicResistance;
        init => _magicResistance = value;
    }

    public int MaximumHp
    {
        get => _maximumHp;
        init => _maximumHp = value;
    }

    public int MaximumMp
    {
        get => _maximumMp;
        init => _maximumMp = value;
    }

    public int SkillDamagePct
    {
        get => _skillDamagePct;
        set => _skillDamagePct = value;
    }

    public int SpellDamagePct
    {
        get => _spellDamagePct;
        set => _spellDamagePct = value;
    }
    
    public int PhysicalAttack
    {
        get => _physicalAttack;
        set => _physicalAttack = value;
    }

    public int MagicAttack
    {
        get => _magicAttack;
        set => _magicAttack = value;
    }
    
    public virtual void Add(Attributes other)
    {
        Interlocked.Add(ref _ac, other.Ac);
        Interlocked.Add(ref _dmg, other.Dmg);
        Interlocked.Add(ref _hit, other.Hit);
        Interlocked.Add(ref _str, other.Str);
        Interlocked.Add(ref _int, other.Int);
        Interlocked.Add(ref _wis, other.Wis);
        Interlocked.Add(ref _con, other.Con);
        Interlocked.Add(ref _dex, other.Dex);
        Interlocked.Add(ref _magicResistance, other.MagicResistance);
        Interlocked.Add(ref _maximumHp, other.MaximumHp);
        Interlocked.Add(ref _maximumMp, other.MaximumMp);
        Interlocked.Add(ref _atkSpeedPct, other.AtkSpeedPct);
        Interlocked.Add(ref _skillDamagePct, other.SkillDamagePct);
        Interlocked.Add(ref _spellDamagePct, other.SpellDamagePct);
        Interlocked.Add(ref _flatSkillDamage, other.FlatSkillDamage);
        Interlocked.Add(ref _flatSpellDamage, other.FlatSpellDamage);
        Interlocked.Add(ref _physicalAttack, other.PhysicalAttack);
        Interlocked.Add(ref _magicAttack, other.MagicAttack);        
    }

    public virtual void Subtract(Attributes other)
    {
        Interlocked.Add(ref _ac, -other.Ac);
        Interlocked.Add(ref _dmg, -other.Dmg);
        Interlocked.Add(ref _hit, -other.Hit);
        Interlocked.Add(ref _str, -other.Str);
        Interlocked.Add(ref _int, -other.Int);
        Interlocked.Add(ref _wis, -other.Wis);
        Interlocked.Add(ref _con, -other.Con);
        Interlocked.Add(ref _dex, -other.Dex);
        Interlocked.Add(ref _magicResistance, -other.MagicResistance);
        Interlocked.Add(ref _maximumHp, -other.MaximumHp);
        Interlocked.Add(ref _maximumMp, -other.MaximumMp);
        Interlocked.Add(ref _atkSpeedPct, -other.AtkSpeedPct);
        Interlocked.Add(ref _skillDamagePct, -other.SkillDamagePct);
        Interlocked.Add(ref _spellDamagePct, -other.SpellDamagePct);
        Interlocked.Add(ref _flatSkillDamage, -other.FlatSkillDamage);
        Interlocked.Add(ref _flatSpellDamage, -other.FlatSpellDamage);
        Interlocked.Add(ref _physicalAttack, -other.PhysicalAttack);
        Interlocked.Add(ref _magicAttack, -other.MagicAttack);
    }
}