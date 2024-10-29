using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class GiantMantisScript : ConfigurableMonsterScriptBase
{
    private readonly Monster Monster;
    private readonly IMonsterFactory MonsterFactory;
    private readonly ISimpleCache SimpleCache;
    private Aisling? player;
    private bool _triggered66Percent = false;
    private bool _triggered33Percent = false;
    private static readonly (int, int)[] Coordinates = [(9, 9), (4, 10), (6, 16), (14, 11), (14, 16)];
    
    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    #endregion
    
    /// <inheritdoc />
    public GiantMantisScript(Monster subject, IMonsterFactory monsterFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        Monster = subject;
        MonsterFactory = monsterFactory;
        SimpleCache = simpleCache;

    }

    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage, int? aggroOverride)
    {
        player ??= source as Aisling;
        var healthPercentage = (float)Subject.StatSheet.CurrentHp / Subject.StatSheet.MaximumHp * 100;

        // Check 66% threshold
        if (!_triggered66Percent && healthPercentage <= 66)
        {
            var mapInstance = SimpleCache.Get<MapInstance>("porteForestPeak");
            var points = GenerateSpawnPoints(2);
            foreach (var point in points)
            {
                var monster = MonsterFactory.Create("guardianOfPorteForest", mapInstance, point);
                mapInstance.AddEntity(monster, point);
            }   
            _triggered66Percent = true;
        }

        // Check 33% threshold
        if (_triggered33Percent || !(healthPercentage <= 25)) return;
        {
            var mapInstance = SimpleCache.Get<MapInstance>("porteForestPeak");
            var points = GenerateSpawnPoints(3);
            foreach (var point in points)
            {
                var monster = MonsterFactory.Create("guardianOfPorteForest", mapInstance, point);
                mapInstance.AddEntity(monster, point);
            }   
            _triggered33Percent = true;
        }
    }

    private static List<Point> GenerateSpawnPoints(int count)
    {
        var random = new Random();
        var selectedPoints = new List<Point>();

        // Ensure the requested count does not exceed available points
        if (count > Coordinates.Length)
        {
            throw new ArgumentException("Requested count exceeds the number of available coordinates.");
        }

        // Select unique points
        var usedIndices = new HashSet<int>();
        while (selectedPoints.Count < count)
        {
            var index = random.Next(Coordinates.Length);
            if (usedIndices.Contains(index)) continue;
            selectedPoints.Add(Coordinates[index]);
            usedIndices.Add(index);
        }
        return selectedPoints;
    }
    
    public override void OnDeath()
    {
        if (player == null) return;
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        var requiredMapId = player?.Trackers.LastMapInstanceId;
        
        foreach (var monster in Monster.MapInstance.GetEntities<Monster>())
        {
            Monster.MapInstance.RemoveEntity(monster);
        }
        
        if (player?.Group == null)
        {
            player?.Trackers.Enums.Set(PorteForestQuestStatus.KilledTheMantis);
            player?.TraverseMap(targetMap, Destination);
            return;
        }
        
        foreach (var aisling in player.Group)
        {
            aisling.Trackers.Enums.Set(PorteForestQuestStatus.KilledTheMantis);
            if (aisling.Trackers.LastMapInstanceId != requiredMapId) continue;
            aisling.TraverseMap(targetMap, Destination);
        }
    }
}
