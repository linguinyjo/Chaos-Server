using Chaos.Formulae.Abstractions;

namespace Chaos.Formulae.LevelRange;

public class LevelRangeFormula : ILevelRangeFormula
{
    /// <inheritdoc />
    public int GetLowerBound(int level) => (int)Math.Ceiling(Math.Max(0, level * 5m - 15) / 6);

    /// <inheritdoc />
    public int GetUpperBound(int level) => (int)Math.Floor(level + level / 5m + 3);

    /// <inheritdoc />
    public bool WithinLevelRange(int level1, int level2)
    {
        var min = Math.Min(level1, level2);
        var max = Math.Max(level1, level2);

        return (min >= GetLowerBound(max)) && (max <= GetUpperBound(min));
    }
}