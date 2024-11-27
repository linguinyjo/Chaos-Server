using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.WindScripts;

public class WindDefensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public WindDefensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Wind";
        if(subject.ScriptKeys.Contains("SetWindDefense")) return;
        subject.AddScript<SetWindDefenseScript>();
    }

    /// <inheritdoc />
    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (!node.Name.StartsWithI("Wind"))
            yield return node with
            {
                Name = $"Wind {node.Name}"
            };
    }
}
