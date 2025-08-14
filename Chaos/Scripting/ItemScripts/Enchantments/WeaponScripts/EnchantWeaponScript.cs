using Chaos.Common.Definitions;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.WeaponScripts;

public abstract class EnchantWeaponScriptBase : ItemScriptBase, IEnchantmentScript
{
    protected abstract int EnchantLevel { get; }
    
    private static readonly Dictionary<LevelCircle, int> BasePhysicalAttackBonus = new()
    {
        { LevelCircle.One, 2 },      // Levels 1-10
        { LevelCircle.Two, 4 },      // Levels 11-40
        { LevelCircle.Three, 8 },   // Levels 41-70
        { LevelCircle.Four, 12 },    // Levels 71-98
        { LevelCircle.Five, 16 },    // Levels 99+
        { LevelCircle.Six, 20 },     // Master
        { LevelCircle.Seven, 30 }    // Advanced Class
    };

    protected EnchantWeaponScriptBase(Item subject) : base(subject)
    {
        Initialize(subject);
    }

    private void Initialize(Item subject)
    {
        ApplyEnchantment(subject);
    }

    protected void ApplyEnchantment(Item subject)
    {
        UpdateEnchantLevel();
        var pAtkBonus = CalculatePhysicalAttackBonus(subject);
        subject.Modifiers.PhysicalAttack += pAtkBonus;
    }

    protected void UpdateEnchantLevel()
    {
        Subject.Prefix = $"+{EnchantLevel}";
    }

    protected int CalculatePhysicalAttackBonus(Item subject)
    {
        var baseBonus = BasePhysicalAttackBonus.GetValueOrDefault(subject.LevelCircle, 4); 
        if (EnchantLevel > 9)
        {
            baseBonus *= 2;
        }
        return baseBonus;
    }

    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (node.Name.StartsWith("+")) yield break;
        if (template.Modifiers != null)
            yield return node with
            {};
    }
}

public class EnchantWeapon1Script : EnchantWeaponScriptBase
{
    /// <inheritdoc />
    public EnchantWeapon1Script(Item subject)
        : base(subject) {}

    protected override int EnchantLevel => 1;
}

public class EnchantWeapon2Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 2;

    /// <inheritdoc />
    public EnchantWeapon2Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon3Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 3;

    /// <inheritdoc />
    public EnchantWeapon3Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon4Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 4;

    /// <inheritdoc />
    public EnchantWeapon4Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon5Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 5;

    /// <inheritdoc />
    public EnchantWeapon5Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon6Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 6;

    /// <inheritdoc />
    public EnchantWeapon6Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon7Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 7;

    /// <inheritdoc />
    public EnchantWeapon7Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon8Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 8;

    /// <inheritdoc />
    public EnchantWeapon8Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon9Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 9;

    /// <inheritdoc />
    public EnchantWeapon9Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon10Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 10;

    /// <inheritdoc />
    public EnchantWeapon10Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon11Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 11;

    /// <inheritdoc />
    public EnchantWeapon11Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon12Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 12;

    /// <inheritdoc />
    public EnchantWeapon12Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon13Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 13;

    /// <inheritdoc />
    public EnchantWeapon13Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon14Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 14;

    /// <inheritdoc />
    public EnchantWeapon14Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon15Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 15;

    /// <inheritdoc />
    public EnchantWeapon15Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon16Script : EnchantWeaponScriptBase
{
    protected override int EnchantLevel => 16;

    /// <inheritdoc />
    public EnchantWeapon16Script(Item subject)
        : base(subject) {}
}
