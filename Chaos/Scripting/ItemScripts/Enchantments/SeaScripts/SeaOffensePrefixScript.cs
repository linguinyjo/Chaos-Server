using Chaos.Common.Definitions;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.SeaScripts;

public class SeaOffensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public SeaOffensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Sea";
        if(subject.ScriptKeys.Contains("SetSeaOffense")) return;
        subject.AddScript<SetSeaOffenseScript>();
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
