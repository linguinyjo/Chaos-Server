using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct IdentifyAbilityComponent : IComponent
{
    private const int LineWidth = 52;
    private const string ColorPrefix = "{=";
    private const int LeftColumnWidth = 26;

    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
    }

    private static string FormatColor(string text, MessageColor color)
    {
        if (color == MessageColor.Default) return text;

        var colorCode = (char)color;
        return $"{ColorPrefix}{colorCode}{text}";
    }

    private static string FormatText(string text, MessageColor color)
    {
        if (color == MessageColor.Default)
            return text;

        var colorCode = (char)color;
        return $"{{={colorCode}{text}";
    }

    private static string FormatTwoColumns(
        string leftLabel,
        string leftValue,
        string rightLabel,
        string rightValue,
        MessageColor leftValueColor = MessageColor.White,
        MessageColor rightValueColor = MessageColor.White,
        MessageColor labelColor = MessageColor.Orange)
    {
        // Combine label and value
        var leftContent = $"{leftLabel}: {leftValue}";
        var rightContent = $"{rightLabel}: {rightValue}";

        // Truncate or pad left column
        leftContent = leftContent.Length > LeftColumnWidth
            ? string.Concat(leftContent.AsSpan(0, LeftColumnWidth - 3), "...")
            : leftContent.PadRight(LeftColumnWidth);

        // Truncate right column if needed
        const int rightMaxWidth = LineWidth - LeftColumnWidth;
        if (rightContent.Length > rightMaxWidth)
        {
            rightContent = string.Concat(rightContent.AsSpan(0, rightMaxWidth - 3), "...");
        }

        var coloredLeft = ApplyColors(leftContent, labelColor, leftValueColor);
        var coloredRight = ApplyColors(rightContent, labelColor, rightValueColor);

        return coloredLeft + coloredRight;
    }

    private static string ApplyColors(string content, MessageColor labelColor, MessageColor valueColor)
    {
        var colonIndex = content.IndexOf(':');
        if (colonIndex == -1) return FormatColor(content, labelColor);

        var label = content[..colonIndex];
        var value = content[colonIndex..];

        return FormatColor(label, labelColor) + FormatColor(value, valueColor);
    }

    private static MessageColor GetElementColor(Element element)
    {
        return element switch
        {
            Element.Fire => MessageColor.Red,
            Element.Water => MessageColor.Silver,
            Element.Wind => MessageColor.Yellow,
            Element.Earth => MessageColor.DarkGreen,
            Element.Holy => MessageColor.HotPink,
            Element.Darkness => MessageColor.Black,
            _ => MessageColor.White
        };
    }
}