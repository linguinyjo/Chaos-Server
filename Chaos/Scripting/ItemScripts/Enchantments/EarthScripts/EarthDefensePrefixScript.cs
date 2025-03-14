using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Scripting.ItemScripts.ElementScripts;

namespace Chaos.Scripting.ItemScripts.Enchantments.EarthScripts;

public class EarthDefensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public EarthDefensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Earth";
        if(subject.ScriptKeys.Contains("SetEarthDefense")) return;
        subject.AddScript<SetEarthDefenseScript>();
    }

    /// <inheritdoc />
    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (!node.Name.StartsWithI("Earth"))
            yield return node with
            {
                Name = $"Earth {node.Name}"
            };
    }
}
