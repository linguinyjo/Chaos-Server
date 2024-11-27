using Chaos.Common.Definitions;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.SeaScripts;

public class SeaDefensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public SeaDefensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Sea";
        if(subject.ScriptKeys.Contains("SetSeaDefense")) return;
        subject.AddScript<SetSeaDefenseScript>();
    }

    /// <inheritdoc />
    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (!node.Name.StartsWithI("Sea"))
            yield return node with
            {
                Name = $"Sea {node.Name}"
            };
    }
}
