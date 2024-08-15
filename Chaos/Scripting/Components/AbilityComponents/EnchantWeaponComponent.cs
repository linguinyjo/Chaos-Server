using Chaos.Common.Definitions;
using Chaos.Extensions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Enchantments;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct EnchantWeaponComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IEnchantWeaponComponentOptions>();
        var item = context.SourceAisling?.Inventory[1];
        if (item == null) return false;
        if (item.LevelCircle != options.LevelCircle)
        {
            context.SourceAisling?.SendOrangeBarMessage($"This scroll can only enchant items of Circle {options.LevelCircle}");
            return false;
        }
        var shouldEnchant = RunEnchantCalculation(item);
        if (shouldEnchant)
        {
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
    private static bool RunEnchantCalculation(Item item)
    {
        switch (item.Enchant)
        {
            case null:
            case < 4:
                return true;
        }

        var random = new Random();
        // Generate a random number between 0 and 99 (inclusive)
        var randomNumber = random.Next(100);

        // Check if the number is less than 66 (66% chance)
        return randomNumber < 66;
    }
        
        
    public interface IEnchantWeaponComponentOptions
    {
        LevelCircle LevelCircle { get; init; }
    }
}
