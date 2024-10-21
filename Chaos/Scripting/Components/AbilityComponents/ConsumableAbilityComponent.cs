using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Enchantments;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct ConsumableAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IConsumableComponentOptions>();
        context.SourceAisling?.Inventory.RemoveQuantity(options.Item.Slot, 1);
    }

    public interface IConsumableComponentOptions
    {
        string ItemName { get; init; }
        // Save the item here in order to get the slot - when the the item is initially constructed it doesnt have a
        // slot value yet so saving the slot byte at this point doesnt work
        Item Item { get; init; }
    }
}