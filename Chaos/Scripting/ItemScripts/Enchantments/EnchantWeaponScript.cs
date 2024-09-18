using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments;

public class EnchantWeaponScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public EnchantWeaponScript(Item subject)
        : base(subject)
    {
        Console.WriteLine(subject);
        if (subject.Enchant == null) subject.Enchant = 1;
        else subject.Enchant += 1;
        
        var pAtkIncrease = (subject.Enchant <= 7) ? 4 : 8;
        Console.WriteLine(pAtkIncrease);
        subject.Modifiers.PhysicalAttack += pAtkIncrease;
        Subject.Prefix = $"+{subject.Enchant}";
    }

    /// <inheritdoc />
    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (node.Name.StartsWith("+")) yield break;
        if (template.Modifiers != null)
            yield return node with
            {
                Name = $"+{template.Enchant} {node.Name}"
            };
    }
}
