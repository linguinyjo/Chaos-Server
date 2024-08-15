using Chaos.Extensions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Enchantments;
using Chaos.TypeMapper.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct EnchantWeaponComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IEnchantWeaponComponentOptions>();
        var item = context.SourceAisling?.Inventory[1];
        if (item == null) return false;
        var shouldEnchant = ShouldEnchant(item);
        if (shouldEnchant)
        {
            // item?.RemoveScript<EnchantWeaponScript>();
            // var script1 = new EnchantWeaponScript(item, item.Modifiers.Enchant);
            item.AddScript<EnchantWeaponScript>();
            context.SourceAisling?.Inventory.Update(1);
            context.SourceAisling?.Client.SendSound(19, false);
        }
        else
        {
            context.SourceAisling?.Inventory.Remove(1);
            context.SourceAisling?.Client.SendSound(10, false);
        }
        return true;
    }

    /// Safe enchant up to 3
    private static bool ShouldEnchant(Item? item)
    {
        if (item?.Enchant == null) return true;
        if (item?.Enchant < 4) return true;
        Random random = new Random();
        // Generate a random number between 0 and 99 (inclusive)
        int randomNumber = random.Next(100);

        // Check if the number is less than 66 (66% chance)
        if (randomNumber < 66)
        {
            return true;  // Probability passed
        }

        return false;  // Probability failed
    }
        
        
    public interface IEnchantWeaponComponentOptions
    {
        string ItemName { get; init; }
    }
}