using System.Runtime.InteropServices;
using Chaos.Common.Definitions;
using Chaos.Common.Synchronization;
using Chaos.Data;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.Geometry.Abstractions;
using Chaos.Objects.World;
using Chaos.Objects.World.Abstractions;
using Chaos.Pathfinding.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripts.MapScripts.Abstractions;
using Chaos.Templates;
using Chaos.Time.Abstractions;
using Chaos.Utilities;

namespace Chaos.Containers;

public sealed class MapInstance : IScripted<IMapScript>, IDeltaUpdatable
{
    private readonly List<MonsterSpawn> MonsterSpawns;
    private readonly MapEntityCollection Objects;
    public MapFlags Flags { get; set; }
    public string InstanceId { get; init; }
    public int? MaximumLevel { get; set; }
    public int? MinimumLevel { get; set; }
    public byte Music { get; set; }
    public string Name { get; set; }
    public IPathfindingService Pathfinder { get; set; } = null!;
    public MapTemplate Template { get; set; }
    /// <inheritdoc />
    public IMapScript Script { get; }

    /// <inheritdoc />
    public ISet<string> ScriptKeys { get; }
    public FifoAutoReleasingSemaphoreSlim Sync { get; }

    public MapInstance(
        MapTemplate template,
        IScriptProvider scriptProvider,
        string name,
        string instanceId,
        ICollection<string>? extraScriptKeys = null
    )
    {
        Name = name;
        InstanceId = instanceId;
        var walkableArea = template.Height * template.Width - template.Tiles.Flatten().Count(t => t.IsWall);
        Objects = new MapEntityCollection(template.Width, template.Height, walkableArea);
        MonsterSpawns = new List<MonsterSpawn>();
        Sync = new FifoAutoReleasingSemaphoreSlim(1, 1);
        Template = template;
        ScriptKeys = new HashSet<string>(template.ScriptKeys, StringComparer.OrdinalIgnoreCase);

        if (extraScriptKeys != null)
            ScriptKeys.AddRange(extraScriptKeys);

        Script = scriptProvider.CreateScript<IMapScript, MapInstance>(ScriptKeys, this);
    }

    public void AddObject(VisibleEntity visibleEntity, IPoint point)
    {
        visibleEntity.SetLocation(this, point);
        Objects.Add(visibleEntity.Id, visibleEntity);

        if (visibleEntity is Creature c)
            Script.OnEntered(c);

        if (visibleEntity is Aisling aisling)
        {
            (var aislings, var doors, var otherVisibles) = Objects.WithinRange<VisibleEntity>(point)
                                                                  .PartitionBySendType();

            foreach (var otherCreature in otherVisibles.OfType<Creature>())
                Helpers.HandleApproach(aisling, otherCreature);

            aisling.Client.SendMapChangePending();
            aisling.Client.SendMapInfo();
            aisling.Client.SendLocation();

            foreach (var nearbyUser in aislings)
            {
                if (nearbyUser.Equals(aisling))
                    continue;

                if (aisling.IsVisibleTo(nearbyUser))
                    nearbyUser.Client.SendDisplayAisling(aisling);

                if (nearbyUser.IsVisibleTo(aisling))
                    aisling.Client.SendDisplayAisling(nearbyUser);
            }

            aisling.Client.SendVisibleObjects(otherVisibles);
            aisling.Client.SendDoors(doors);
            aisling.Client.SendMapChangeComplete();
            aisling.Client.SendSound(Music, true);
            aisling.Client.SendMapLoadComplete();
            aisling.Client.SendDisplayAisling(aisling);
        } else
        {
            //fast path for non creatures
            if (visibleEntity is not Creature creature)
            {
                foreach (var nearbyAisling in Objects.WithinRange<Aisling>(visibleEntity)
                                                     .ThatCanSee(visibleEntity))
                    nearbyAisling.Client.SendVisibleObjects(visibleEntity);

                return;
            }

            foreach (var nearbyCreature in Objects.WithinRange<Creature>(creature))
            {
                if (nearbyCreature.Equals(creature))
                    continue;

                Helpers.HandleApproach(creature, nearbyCreature);

                if (nearbyCreature is Aisling nearbyAisling && creature.IsVisibleTo(nearbyCreature))
                    nearbyAisling.Client.SendVisibleObjects(visibleEntity);
            }
        }
    }

    public void AddObjects<T>(ICollection<T> visibleObjects) where T: VisibleEntity
    {
        if (visibleObjects.Count == 0)
            return;

        foreach (var visibleObj in visibleObjects)
        {
            if (visibleObj is Creature creature)
                Script.OnEntered(creature);

            visibleObj.MapInstance = this;
            Objects.Add(visibleObj.Id, visibleObj);
        }

        foreach (var creature in Objects.Values<Creature>())
        {
            var objectsInRange = visibleObjects
                                 .ThatAreWithinRange(creature)
                                 .ToList();

            foreach (var nearbyCreature in objectsInRange.OfType<Creature>())
                Helpers.HandleApproach(creature, nearbyCreature);

            if (creature is Aisling aisling && objectsInRange.Any())
                aisling.Client.SendVisibleObjects(objectsInRange.ThatAreVisibleTo(aisling));
        }
    }

    public void AddSpawner(MonsterSpawn monsterSpawn)
    {
        monsterSpawn.MapInstance = this;

        monsterSpawn.SpawnArea ??= new Rectangle(
            0,
            0,
            Template.Width,
            Template.Height);

        MonsterSpawns.Add(monsterSpawn);
    }

    public void Click(uint id, Aisling source)
    {
        if (TryGetObject<VisibleEntity>(id, out var obj))
            if (obj.WithinRange(source) && obj.IsVisibleTo(source))
                obj.OnClicked(source);
    }

    public void Click(IPoint point, Aisling source)
    {
        var door = Objects.AtPoint<Door>(point)
                          .TopOrDefault();

        if (door != null)
            door.OnClicked(source);
        else
        {
            var obj = Objects.AtPoint<ReactorTile>(point)
                             .TopOrDefault();

            obj?.OnClicked(source);
        }
    }

    public void Destroy()
    {
        Objects.Clear();
        MonsterSpawns.Clear();
    }

    public IEnumerable<T> GetEntities<T>() where T: MapEntity => Objects.Values<T>();

    public IEnumerable<T> GetEntitiesAtPoint<T>(IPoint point) where T: MapEntity => Objects.AtPoint<T>(point);

    public IEnumerable<T> GetEntitiesAtPoints<T>(IEnumerable<IPoint> points) where T: MapEntity => Objects.AtPoints<T>(points);

    public IEnumerable<T> GetEntitiesWithinRange<T>(IPoint point, int range = 13) where T: MapEntity =>
        Objects.WithinRange<T>(point, range);

    public bool IsWalkable(IPoint point, CreatureType creatureType)
    {
        var creatures = Objects.AtPoint<Creature>(point)
                               .ToList();

        return creatureType switch
        {
            CreatureType.Normal => !IsWallToCreaturesOnly(point)
                                   && !IsWall(point)
                                   && !creatures.Any(c => c.Type.WillCollideWith(creatureType)),
            CreatureType.WalkThrough => !IsWallToCreaturesOnly(point) && !creatures.Any(c => c.Type.WillCollideWith(creatureType)),
            CreatureType.Merchant => !IsWallToCreaturesOnly(point)
                                     && !IsWall(point)
                                     && !creatures.Any(c => c.Type.WillCollideWith(creatureType)),
            CreatureType.WhiteSquare => !IsWallToCreaturesOnly(point)
                                        && !IsWall(point)
                                        && !creatures.Any(c => c.Type.WillCollideWith(creatureType)),
            CreatureType.Aisling => !IsWall(point) && !creatures.Any(c => c.Type.WillCollideWith(creatureType)),
            _                    => throw new ArgumentOutOfRangeException(nameof(creatureType), creatureType, null)
        };
    }

    public bool IsWall(IPoint point)
    {
        if (Template.IsWall(point))
            return true;

        var door = GetEntitiesAtPoint<Door>(point).FirstOrDefault();

        return door?.Closed ?? false;
    }

    public bool IsWallToCreaturesOnly(IPoint point) => Objects.AtPoint<ReactorTile>(point).Any(rt => rt.ShouldBlockPathfinding);

    public bool IsWithinMap(IPoint point) => Template.IsWithinMap(point);

    /// <summary>
    ///     Moves an entity within the point lookup of the master object collection. DO NOT USE THIS UNLESS YOU KNOW WHAT YOU ARE DOING.
    /// </summary>
    public void MoveEntity(MapEntity entity, Point oldPoint) => Objects.MoveEntity(entity, oldPoint);

    public void PlayMusic(byte music)
    {
        foreach (var aisling in Objects.Values<Aisling>())
            aisling.Client.SendSound(music, true);
    }

    public void PlaySound(byte sound, IPoint point)
    {
        foreach (var aisling in Objects.WithinRange<Aisling>(point))
            aisling.Client.SendSound(sound, false);
    }

    public void PlaySound(byte sound, ICollection<IPoint> points)
    {
        var aislings = Objects.Values<Aisling>()
                              .Where(aisling => points.Any(p => p.WithinRange(aisling)));

        foreach (var aisling in aislings)
            aisling.Client.SendSound(sound, false);
    }

    public bool RemoveObject(MapEntity mapEntity)
    {
        if (!Objects.Remove(mapEntity.Id))
            return false;

        if (mapEntity is VisibleEntity visibleObject)
            foreach (var aisling in Objects.WithinRange<Aisling>(visibleObject)
                                           .ThatCanSee(visibleObject))
                aisling.Client.SendRemoveObject(visibleObject.Id);

        if (mapEntity is Creature creature)
        {
            Script.OnExited(creature);

            foreach (var nearbyCreature in Objects.WithinRange<Creature>(creature))
                Helpers.HandleDeparture(creature, nearbyCreature);
        }

        return true;
    }

    public void ShowAnimation(Animation animation)
    {
        if (animation.TargetId.HasValue)
        {
            if (TryGetObject<Creature>(animation.TargetId.Value, out var target))
                foreach (var aisling in Objects.WithinRange<Aisling>(target)
                                               .ThatCanSee(target))
                    aisling.Client.SendAnimation(animation);
        } else if (animation.TargetPoint != default)
            foreach (var aisling in Objects.WithinRange<Aisling>(animation.TargetPoint))
                aisling.Client.SendAnimation(animation);
    }

    public void SimpleAdd(MapEntity mapEntity)
    {
        mapEntity.MapInstance = this;
        Objects.Add(mapEntity.Id, mapEntity);
    }

    /// <inheritdoc />
    public override string ToString() =>
        $"{{ Type: \"{nameof(MapInstance)}\", InstanceId: \"{InstanceId}\", TemplateId: {Template.MapId} }}";

    public bool TryGetObject<T>(uint id, [MaybeNullWhen(false)] out T obj) => Objects.TryGetValue(id, out obj);

    public void Update(TimeSpan delta)
    {
        Objects.Update(delta);

        foreach (ref var spawn in CollectionsMarshal.AsSpan(MonsterSpawns))
            spawn.Update(delta);

        Script.Update(delta);
    }
}