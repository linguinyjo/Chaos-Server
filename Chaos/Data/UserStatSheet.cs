// ReSharper disable InconsistentNaming

using System.Threading;
using Chaos.Common.Definitions;

namespace Chaos.Data;

public record UserStatSheet : StatSheet
{
    protected AdvClass _advClass;
    protected BaseClass _baseClass;
    // ReSharper disable once UnassignedField.Global
    protected int _currentWeight;
    protected bool _master;
    protected int _maxWeight;
    protected long _toNextAbility;
    protected long _toNextLevel;
    protected long _totalAbility;
    protected long _totalExp;
    protected int _unspentPoints;

    public AdvClass AdvClass
    {
        get => _advClass;
        init => _advClass = value;
    }

    public BaseClass BaseClass
    {
        get => _baseClass;
        init => _baseClass = value;
    }

    public int CurrentWeight
    {
        get => _currentWeight;
        init => _currentWeight = value;
    }

    public bool Master
    {
        get => _master;
        init => _master = value;
    }

    public int MaxWeight
    {
        get => _maxWeight;
        init => _maxWeight = value;
    }

    public static UserStatSheet NewCharacter => new()
    {
        _maxWeight = 40,
        _toNextLevel = 100,
        _str = 1,
        _int = 1,
        _wis = 1,
        _con = 1,
        _dex = 1,
        _currentHp = 100,
        _maximumHp = 100,
        _currentMp = 50,
        _maximumMp = 50,
        _level = 1,
        _master = false,
        _baseClass = BaseClass.Peasant,
        _advClass = AdvClass.None
    };

    public uint ToNextAbility
    {
        get => Convert.ToUInt32(_toNextAbility);
        init => _toNextAbility = value;
    }

    public uint ToNextLevel
    {
        get => Convert.ToUInt32(_toNextLevel);
        init => _toNextLevel = value;
    }

    public uint TotalAbility
    {
        get => Convert.ToUInt32(_totalAbility);
        init => _totalAbility = value;
    }

    public uint TotalExp
    {
        get => Convert.ToUInt32(_totalExp);
        init => _totalExp = value;
    }

    public int UnspentPoints
    {
        get => _unspentPoints;
        init => _unspentPoints = value;
    }

    public void AddTNA(long amount) => Interlocked.Add(ref _toNextAbility, amount);

    public void AddTNL(long amount) => Interlocked.Add(ref _toNextLevel, amount);

    public void AddTotalAbility(long amount) => Interlocked.Add(ref _totalAbility, amount);

    public void AddTotalExp(long amount) => Interlocked.Add(ref _totalExp, amount);

    public void AddWeight(int amount) => Interlocked.Add(ref _currentWeight, amount);

    public void IncrementLevel()
    {
        Interlocked.Increment(ref _level);
        Interlocked.Increment(ref _unspentPoints);
        RecalculateMaxWeight();
    }

    public bool IncrementStat(Stat stat)
    {
        //if it's 0, do nothing
        if (_unspentPoints == 0)
            return false;

        if (Interlocked.Decrement(ref _unspentPoints) < 0)
        {
            _unspentPoints = 0;

            return false;
        }

        switch (stat)
        {
            case Stat.STR:
                Interlocked.Increment(ref _str);
                RecalculateMaxWeight();

                break;
            case Stat.INT:
                Interlocked.Increment(ref _int);

                break;
            case Stat.WIS:
                Interlocked.Increment(ref _wis);

                break;
            case Stat.CON:
                Interlocked.Increment(ref _con);

                break;
            case Stat.DEX:
                Interlocked.Increment(ref _dex);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }

        return true;
    }

    private void RecalculateMaxWeight() => _maxWeight = 40 + _level + _str;

    public void SetAdvClass(AdvClass advClass) => _advClass = advClass;

    public void SetBaseClass(BaseClass baseClass) => _baseClass = baseClass;
}