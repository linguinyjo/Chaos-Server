using Chaos.Models.Data;
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
        // Loop through the inventory
        if (context.SourceAisling?.Inventory == null) return;
        foreach (var item in context.SourceAisling.Inventory)
        {
            if (item.UniqueId != options.UniqueId) continue;
            context.SourceAisling?.Inventory.RemoveQuantity(item.Slot, 1);
            return;
        }
    }

    public interface IConsumableComponentOptions
    {
        string ItemName { get; init; }
        ulong UniqueId { get; init; }
    }
}