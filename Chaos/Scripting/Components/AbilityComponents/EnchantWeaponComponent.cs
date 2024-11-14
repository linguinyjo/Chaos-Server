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
        
        if (item.LevelCircle != options.LevelCircle || item.Template.EquipmentType != EquipmentType.Weapon)
        {
            context.SourceAisling?.SendOrangeBarMessage($"This scroll can only enchant weapons of Circle {options.LevelCircle}");
            return false;
        }
        var enchantLevel = GetEnchantLevelFromPrefix(item);
        var shouldEnchant = RunEnchantCalculation(item, enchantLevel);
        if (shouldEnchant)
        {
            AddScript(item, enchantLevel);
            context.SourceAisling?.Inventory.Update(item.Slot);
            context.SourceAisling?.Client.SendSound(19, false);
            context.SourceAisling?.SendOrangeBarMessage($"Successfully enchanted Your {item.Template.Name} to +{enchantLevel+1}");
        }
        else
        {
            context.SourceAisling?.Inventory.Remove(item.Slot);
            context.SourceAisling?.Client.SendSound(10, false);
            context.SourceAisling?.SendOrangeBarMessage($"Your {item.Template.Name} has smashed into a thousand pieces");
        }
        return true;
    }

    private static void AddScript(Item item, int enchantLevel)
    {
        switch (enchantLevel)
        {
            case 0: 
                item.AddScript<EnchantWeapon1Script>();
                break;
            case 1: 
                item.AddScript<EnchantWeapon2Script>();
                break;
            case 2: 
                item.AddScript<EnchantWeapon3Script>();
                break;
            case 3: 
                item.AddScript<EnchantWeapon4Script>();
                break;
            case 4: 
                item.AddScript<EnchantWeapon5Script>();
                break;
            case 5: 
                item.AddScript<EnchantWeapon6Script>();
                break;
            case 6: 
                item.AddScript<EnchantWeapon7Script>();
                break;
            case 7: 
                item.AddScript<EnchantWeapon8Script>();
                break;
            case 8: 
                item.AddScript<EnchantWeapon9Script>();
                break;
            case 9: 
                item.AddScript<EnchantWeapon10Script>();
                break;
            case 10: 
                item.AddScript<EnchantWeapon11Script>();
                break;
            case 11: 
                item.AddScript<EnchantWeapon12Script>();
                break;
            case 12: 
                item.AddScript<EnchantWeapon13Script>();
                break;
            case 13: 
                item.AddScript<EnchantWeapon14Script>();
                break;
            case 14: 
                item.AddScript<EnchantWeapon15Script>();
                break;
            case 15: 
                item.AddScript<EnchantWeapon16Script>();
                break;
        }
    }

    private static int GetEnchantLevelFromPrefix(Item subject)
    {
        return int.TryParse(subject.Prefix?.TrimStart('+'), out var enchantLevel) ? enchantLevel : 0;
    }

    /// Safe enchant up to 3, 66% chance to succeed after that
    private static bool RunEnchantCalculation(Item item, int enchantLevel)
    {
        if(enchantLevel < 4) return true;

        var random = new Random();
        var randomNumber = random.Next(100);
        return randomNumber < 66;
    }
        
        
    public interface IEnchantWeaponComponentOptions
    {
        LevelCircle LevelCircle { get; init; }
    }
}
