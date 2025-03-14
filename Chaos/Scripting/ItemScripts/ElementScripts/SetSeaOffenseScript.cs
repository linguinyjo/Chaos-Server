using Chaos.Common.Definitions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.ElementScripts;

public class SetSeaOffenseScript : ItemScriptBase
{
    /// <inheritdoc />
    public SetSeaOffenseScript(Item subject)
        : base(subject) { }

    /// <inheritdoc />
    public override void OnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetOffenseElement(Element.Water);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }

    /// <inheritdoc />
    public override void OnUnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetOffenseElement(Element.None);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }
}