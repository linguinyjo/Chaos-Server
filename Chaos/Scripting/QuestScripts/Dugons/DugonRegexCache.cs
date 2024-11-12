using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public static partial class DugonRegexCache
{
    [GeneratedRegex(@"Sabonim I understand my inner potential", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex WhiteDugonRegex();
    public static readonly ICollection<Regex> WHITE_DUGON_PATTERNS = ImmutableList.Create(WhiteDugonRegex());
    
}