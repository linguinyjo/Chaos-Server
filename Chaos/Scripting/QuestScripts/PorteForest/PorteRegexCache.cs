using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public static partial class RegexCache
{
    [GeneratedRegex(@"Porte Forest", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex PorteForestRegex();
    public static readonly ICollection<Regex> PORTE_FOREST_PATTERNS = ImmutableList.Create(PorteForestRegex());
}