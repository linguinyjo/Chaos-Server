using Chaos.Common.Definitions;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.ArmorScripts;

public abstract class EnchantArmorScriptBase : ItemScriptBase, IEnchantmentScript
{
    protected abstract int EnchantLevel { get; }
    
    private static readonly Dictionary<LevelCircle, int> Bonus = new()
    {
        { LevelCircle.One, 25 },      // Levels 1-10
        { LevelCircle.Two, 30 },      // Levels 11-40
        { LevelCircle.Three, 60 },   // Levels 41-70
        { LevelCircle.Four, 90 },    // Levels 71-98
        { LevelCircle.Five, 10 },    // Levels 99+
        { LevelCircle.Six, 12 },     // Master
        { LevelCircle.Seven, 14 }    // Advanced Class
    };

    protected EnchantArmorScriptBase(Item subject) : base(subject)
    {
        Initialize(subject);
    }

    private void Initialize(Item subject)
    {
        ApplyEnchantment(subject);
    }

    private void ApplyEnchantment(Item subject)
    {
        UpdateEnchantLevel();
        var bonus = CalculateBonus(subject);
        subject.Modifiers.Add(new Attributes { MaximumHp = bonus });
        subject.Modifiers.Add(new Attributes { MaximumMp = bonus });
    }

    private void UpdateEnchantLevel()
    {
        Subject.Prefix = $"+{EnchantLevel}";
    }

    private static int CalculateBonus(Item subject)
    {
        return Bonus.GetValueOrDefault(subject.LevelCircle, 50); 
    }

    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (node.Name.StartsWith("+")) yield break;
        if (template.Modifiers != null)
            yield return node with
            {};
    }
}

public class EnchantArmor1Script : EnchantArmorScriptBase
{
    /// <inheritdoc />
    public EnchantArmor1Script(Item subject)
        : base(subject) {}

    protected override int EnchantLevel => 1;
}

public class EnchantArmor2Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 2;

    /// <inheritdoc />
    public EnchantArmor2Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor3Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 3;

    /// <inheritdoc />
    public EnchantArmor3Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor4Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 4;

    /// <inheritdoc />
    public EnchantArmor4Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor5Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 5;

    /// <inheritdoc />
    public EnchantArmor5Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor6Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 6;

    /// <inheritdoc />
    public EnchantArmor6Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor7Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 7;

    /// <inheritdoc />
    public EnchantArmor7Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor8Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 8;

    /// <inheritdoc />
    public EnchantArmor8Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor9Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 9;

    /// <inheritdoc />
    public EnchantArmor9Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor10Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 10;

    /// <inheritdoc />
    public EnchantArmor10Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor11Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 11;

    /// <inheritdoc />
    public EnchantArmor11Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor12Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 12;

    /// <inheritdoc />
    public EnchantArmor12Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor13Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 13;

    /// <inheritdoc />
    public EnchantArmor13Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor14Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 14;

    /// <inheritdoc />
    public EnchantArmor14Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor15Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 15;

    /// <inheritdoc />
    public EnchantArmor15Script(Item subject)
        : base(subject) {}
}

public class EnchantArmor16Script : EnchantArmorScriptBase
{
    protected override int EnchantLevel => 16;

    /// <inheritdoc />
    public EnchantArmor16Script(Item subject)
        : base(subject) {}
}
