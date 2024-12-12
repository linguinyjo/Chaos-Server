using Chaos.Collections;
using Chaos.Collections.Common;
using Chaos.Extensions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.DialogScripts;

public class RemoveEnchantsScript : DialogScriptBase
{
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public RemoveEnchantsScript(Dialog subject, ISimpleCache simpleCache)
        : base(subject)
        => SimpleCache = simpleCache;

    /// <inheritdoc />
    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var item = source.Inventory[1];
        if (item == null)
        {
            Subject.Reply(
                source,
                "Make sure you have the item you want me to remove the enchants from in your first inventory slot",
                "close"
                );
            return;
        }
        
        foreach (var itemScriptKey in item.ScriptKeys)
        {
            if (itemScriptKey == "Equipment") continue;
            item.ScriptKeys.Remove(itemScriptKey);
        }
        item.Prefix = null;
        // TODO figure out how to refresh the item
        Subject.Reply(
            source,
            "I have removed all enchantments from the item (unequip and re-equip the item to see the effect)",
            "close"
        );
    }
}