using Chaos.Common.Definitions;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments;

public abstract class EnchantWeaponScript : ItemScriptBase, IEnchantmentScript
{
    private static readonly Dictionary<LevelCircle, int> BasePhysicalAttackBonus = new()
    {
        { LevelCircle.One, 2 },      // Levels 1-10
        { LevelCircle.Two, 4 },      // Levels 11-40
        { LevelCircle.Three, 6 },   // Levels 41-70
        { LevelCircle.Four, 8 },    // Levels 71-98
        { LevelCircle.Five, 10 },    // Levels 99+
        { LevelCircle.Six, 12 },     // Master
        { LevelCircle.Seven, 14 }    // Advanced Class
    };

    protected EnchantWeaponScript(Item subject) : base(subject)
    {
        Initialize(subject);
    }

    private void Initialize(Item subject)
    {
        ApplyEnchantment(subject);
    }

    protected virtual void ApplyEnchantment(Item subject)
    {
        UpdateEnchantLevel(subject);
        var pAtkBonus = CalculatePhysicalAttackBonus(subject);
        subject.Modifiers.PhysicalAttack += pAtkBonus;
    }

    protected void UpdateEnchantLevel(Item subject)
    {
        if (subject.Enchant == null) subject.Enchant = 1;
        else subject.Enchant += 1;
        
        Subject.Prefix = $"+{subject.Enchant}";
    }

    protected int CalculatePhysicalAttackBonus(Item subject)
    {
        var baseBonus = BasePhysicalAttackBonus.GetValueOrDefault(subject.LevelCircle, 4); 
        if (subject.Enchant > 10)
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
            {
                Name = $"+{template.Enchant} {node.Name}",
            };
    }
}

public class EnchantWeapon1Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon1Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon2Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon2Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon3Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon3Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon4Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon4Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon5Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon5Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon6Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon6Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon7Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon7Script(Item subject)
        : base(subject) {}
}

public class EnchantWeapon8Script : EnchantWeaponScript
{
    /// <inheritdoc />
    public EnchantWeapon8Script(Item subject)
        : base(subject) {}
}