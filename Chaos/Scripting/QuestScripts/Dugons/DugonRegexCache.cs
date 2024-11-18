using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public static partial class DugonRegexCache
{
    [GeneratedRegex(@"Sabonim, I understand my inner potential", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex WhiteDugonRegex();
    public static readonly ICollection<Regex> WHITE_DUGON_PATTERNS = ImmutableList.Create(WhiteDugonRegex());
    
    [GeneratedRegex(@"Sabonim, I understand the path of growth", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex GreenDugonRegex();
    public static readonly ICollection<Regex> GREEN_DUGON_PATTERNS = ImmutableList.Create(GreenDugonRegex());
    
}

// Green (Growth/Foundation):
// "Sabonim, I understand the path of growth"
// Blue (Adaptability/Flow):
// "Sabonim, I understand how to flow like water"
// Yellow (Awareness/Intelligence):
// "Sabonim, I understand the clarity of mind"
// Purple (Synthesis/Integration):
// "Sabonim, I understand the unity of body and spirit"
// Brown (Stability/Endurance):
// "Sabonim, I understand the strength of patience"
// Red (Power/Intensity):
// "Sabonim, I understand the fire within"
// Black (Transcendence/Harmony):
// "Sabonim, I understand the endless journey"