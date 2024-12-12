using Chaos.Common.Definitions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.Enchantments.EarthScripts;

public class SetEarthDefenseScript : ItemScriptBase
{
    /// <inheritdoc />
    public SetEarthDefenseScript(Item subject)
        : base(subject) { }

    /// <inheritdoc />
    public override void OnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetDefenseElement(Element.Earth);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }

    /// <inheritdoc />
    public override void OnUnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetDefenseElement(Element.None);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }
}