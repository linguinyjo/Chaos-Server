using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.MetaData.ItemMetaData;
using Chaos.Models.Panel;
using Chaos.Models.Templates;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Scripting.ItemScripts.Enchantments.EarthScripts;

namespace Chaos.Scripting.ItemScripts.Enchantments.FireScripts;

public class FireOffensePrefixScript : ItemScriptBase, IEnchantmentScript
{
    /// <inheritdoc />
    public FireOffensePrefixScript(Item subject)
        : base(subject)
    {
        Subject.Prefix = "Fire";
        if(subject.ScriptKeys.Contains("SetFireOffense")) return;
        subject.AddScript<SetFireOffenseScript>();
    }

    /// <inheritdoc />
    public static IEnumerable<ItemMetaNode> Mutate(ItemMetaNode node, ItemTemplate template)
    {
        if (!node.Name.StartsWithI("Fire"))
            yield return node with
            {
                Name = $"Fire {node.Name}"
            };
    }
}
