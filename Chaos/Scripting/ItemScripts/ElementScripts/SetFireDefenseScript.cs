using Chaos.DarkAges.Definitions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts.ElementScripts;

public class SetFireDefenseScript : ItemScriptBase
{
    /// <inheritdoc />
    public SetFireDefenseScript(Item subject)
        : base(subject)
    {
    }

    /// <inheritdoc />
    public override void OnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetDefenseElement(Element.Fire);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }

    /// <inheritdoc />
    public override void OnUnEquipped(Aisling aisling)
    {
        aisling.StatSheet.SetDefenseElement(Element.None);
        aisling.Client.SendAttributes(StatUpdateType.Secondary);
    }
}