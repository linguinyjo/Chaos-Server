using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Scripting.SpellScripts.Abstractions;

namespace Chaos.Scripting.SpellScripts;

public class IdentifyScript : SpellScriptBase
{
    /// <inheritdoc />
    public IdentifyScript(Spell subject)
        : base(subject)
    {
    }
    
    private const string ColorPrefix = "{=";

    public override void OnUse(SpellContext context)
    {
        var item = context.SourceAisling?.Inventory[1];
        if (item == null) return;
        var lines = FormatItemDetails(item);
        context.SourceAisling?.SendServerMessage(ServerMessageType.ScrollWindow, string.Join("\n", lines));
    }
    
    private static string FormatItemDetails(Item item)
    {
        var reqClass = item.Template.Class.GetValueOrDefault();
        var lines = new List<string>
        {
            FormatColor("Name: ", MessageColor.Silver) + FormatColor($"{item.DisplayName}", MessageColor.White),
            " ",
            FormatColor("Insight: ", MessageColor.Silver) + FormatColor($"{item.Template.Level}", MessageColor.White) + 
            "  " + 
            FormatColor("Class: ", MessageColor.Silver) + FormatColor($"{reqClass}", MessageColor.White)
        };

        if(item.Template.EquipmentType is EquipmentType.Weapon)
        {
            var patk = FormatColor("Physical Atk: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.PhysicalAttack}", MessageColor.White);
            var matk = FormatColor("Magic Atk: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.MagicAttack}", MessageColor.White);
            lines.Add($"{patk}  {matk}");
        }

        lines.AddRange(new[]
        {
            "",
            FormatColor("Health: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.MaximumHp}", MessageColor.White) + 
            "  " + 
            FormatColor("Mana: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.MaximumMp}", MessageColor.White),

            FormatColor("AC: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Ac}", MessageColor.White) + 
            "  " + 
            FormatColor("MR: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.MagicResistance}", MessageColor.White) + 
            "  " + 
            FormatColor("Regen: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Regen}", MessageColor.White),

            FormatColor("Hit: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Hit}", MessageColor.White) + 
            "  " + 
            FormatColor("Dmg: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Dmg}", MessageColor.White),

            FormatColor("Str: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Str}", MessageColor.White) + 
            "  " + 
            FormatColor("Int: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Int}", MessageColor.White) + 
            "  " + 
            FormatColor("Wis: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Wis}", MessageColor.White) + 
            "  " + 
            FormatColor("Con: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Con}", MessageColor.White) + 
            "  " + 
            FormatColor("Dex: ", MessageColor.Silver) + FormatColor($"{item.Modifiers.Dex}", MessageColor.White),

            FormatColor("Weight: ", MessageColor.Silver) + FormatColor($"{item.Weight}", MessageColor.White) + 
            "  " + 
            FormatColor("Durability: ", MessageColor.Silver) + FormatColor($"{item.Template.MaxDurability}", MessageColor.White) + 
            "  " + 
            FormatColor("Value: ", MessageColor.Silver) + FormatColor($"{item.Template.SellValue}", MessageColor.White),
        });
        return string.Join("\n", lines);
    }

    private static string FormatColor(string text, MessageColor color)
    {
        if (color == MessageColor.Default) return text;
    
        var colorCode = (char)color;
        return $"{ColorPrefix}{colorCode}{text}";
    }
}