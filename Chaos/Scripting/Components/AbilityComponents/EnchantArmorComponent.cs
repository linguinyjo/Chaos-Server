using Chaos.DarkAges.Definitions;
using Chaos.Extensions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Enchantments.ArmorScripts;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct EnchantArmorComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IEnchantArmorComponentOptions>();
        var item = context.SourceAisling?.Inventory[1];
        if (item == null) return false;

        if (item.LevelCircle != options.LevelCircle ||
            item.Template.EquipmentType is not (EquipmentType.Armor or EquipmentType.Gauntlet or EquipmentType.Greaves
                or EquipmentType.Boots)
           )
        {
            context.SourceAisling?.SendOrangeBarMessage(
                $"This scroll can only enchant armor of Circle {options.LevelCircle}");
            return false;
        }

        var enchantLevel = GetEnchantLevelFromPrefix(item);
        var shouldEnchant = RunEnchantCalculation(item, enchantLevel);
        if (shouldEnchant)
        {
            AddScript(item, enchantLevel);
            context.SourceAisling?.Inventory.Update(item.Slot);
            context.SourceAisling?.Client.SendSound(19, false);
            context.SourceAisling?.SendOrangeBarMessage(
                $"Successfully enchanted Your {item.Template.Name} to +{enchantLevel + 1}");
        }
        else
        {
            context.SourceAisling?.Inventory.Remove(item.Slot);
            context.SourceAisling?.Client.SendSound(10, false);
            context.SourceAisling?.SendOrangeBarMessage(
                $"Your {item.Template.Name} has shattered into a thousand pieces");
        }

        return true;
    }

    private static void AddScript(Item item, int enchantLevel)
    {
        switch (enchantLevel)
        {
            case 0:
                item.AddScript<EnchantArmor1Script>();
                break;
            case 1:
                item.AddScript<EnchantArmor2Script>();
                break;
            case 2:
                item.AddScript<EnchantArmor3Script>();
                break;
            case 3:
                item.AddScript<EnchantArmor4Script>();
                break;
            case 4:
                item.AddScript<EnchantArmor5Script>();
                break;
            case 5:
                item.AddScript<EnchantArmor6Script>();
                break;
            case 6:
                item.AddScript<EnchantArmor7Script>();
                break;
            case 7:
                item.AddScript<EnchantArmor8Script>();
                break;
            case 8:
                item.AddScript<EnchantArmor9Script>();
                break;
            case 9:
                item.AddScript<EnchantArmor10Script>();
                break;
            case 10:
                item.AddScript<EnchantArmor11Script>();
                break;
            case 11:
                item.AddScript<EnchantArmor12Script>();
                break;
            case 12:
                item.AddScript<EnchantArmor13Script>();
                break;
            case 13:
                item.AddScript<EnchantArmor14Script>();
                break;
            case 14:
                item.AddScript<EnchantArmor15Script>();
                break;
            case 15:
                item.AddScript<EnchantArmor16Script>();
                break;
        }
    }

    private static int GetEnchantLevelFromPrefix(Item subject)
    {
        return int.TryParse(subject.Prefix?.TrimStart('+'), out var enchantLevel) ? enchantLevel : 0;
    }

    /// Safe enchant up to 3, 60% chance to succeed after that
    private static bool RunEnchantCalculation(Item item, int enchantLevel)
    {
        if (enchantLevel < 4) return true;

        var random = new Random();
        var randomNumber = random.Next(100);
        return randomNumber < 60;
    }


    public interface IEnchantArmorComponentOptions
    {
        LevelCircle LevelCircle { get; init; }
    }
}