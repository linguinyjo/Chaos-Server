using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Scripting.ItemScripts.ElementScripts;

namespace Chaos.Scripting.ItemScripts.Enchantments.WindScripts;

public class WindOffensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public WindOffensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Wind";
        if(subject.ScriptKeys.Contains("SetWindOffense")) return;
        subject.AddScript<SetWindOffenseScript>();
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
