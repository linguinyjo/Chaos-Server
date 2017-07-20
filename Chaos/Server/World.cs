﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Chaos.Objects;
using System;
using System.IO;

namespace Chaos
{
    internal class World
    {
        private string MapKey => "edl396yhvnw85b6kd8vnsj296hj285bq";
        internal Server Server { get; }
        internal ConcurrentDictionary<ushort, Map> Maps { get; set; }
        internal ConcurrentDictionary<uint, WorldMap> WorldMaps { get; set; }
        internal ConcurrentDictionary<int, Group> Groups { get; set; }
        internal ConcurrentDictionary<string, Guild> Guilds { get; set; }
        internal ConcurrentDictionary<int, Exchange> Exchanges { get; set; }
        
        internal World(Server server)
        {
            Server = server;
            Maps = new ConcurrentDictionary<ushort, Map>();
            WorldMaps = new ConcurrentDictionary<uint, WorldMap>();
            Groups = new ConcurrentDictionary<int, Group>();
            Guilds = new ConcurrentDictionary<string, Guild>(StringComparer.CurrentCultureIgnoreCase);
            Exchanges = new ConcurrentDictionary<int, Exchange>();
        }

        internal void Load()
        {
            using (BinaryReader reader = new BinaryReader(new MemoryStream(Server.DataBase.Cache.Get<byte[]>(MapKey))))
            {
                reader.ReadInt32();

                //load worldmaps
                ushort worldMapCount = reader.ReadUInt16();
                for (int wMap = 0; wMap < worldMapCount; ++wMap)
                {
                    WorldMap worldMap = new WorldMap(reader.ReadString(), new WorldMapNode[0]);

                    byte nodeCount = reader.ReadByte();
                    for (int i = 0; i < nodeCount; i++)
                    {
                        ushort x = reader.ReadUInt16();
                        ushort y = reader.ReadUInt16();
                        string name = reader.ReadString();
                        ushort mapId = reader.ReadUInt16();
                        byte dX = reader.ReadByte();
                        byte dY = reader.ReadByte();
                        worldMap.Nodes.Add(new WorldMapNode(new Point(x, y), name, mapId, new Point(dX, dY)));
                    }

                    uint crc32 = worldMap.GetCrc32();
                    WorldMaps[crc32] = worldMap;
                }

                //load maps
                ushort mapCount = reader.ReadUInt16();
                for (int map = 0; map < mapCount; map++)
                {
                    //load map information
                    ushort mapId = reader.ReadUInt16();
                    byte sizeX = reader.ReadByte();
                    byte sizeY = reader.ReadByte();
                    string name = reader.ReadString();
                    MapFlags flags = (MapFlags)reader.ReadUInt32();
                    sbyte music = reader.ReadSByte();
                    Map newMap = new Map(mapId, sizeX, sizeY, flags, name, music);

                    //load doors
                    byte doorCount = reader.ReadByte();
                    for (byte b = 0; b < doorCount; b++)
                    {
                        ushort x = reader.ReadUInt16();
                        ushort y = reader.ReadUInt16();
                        bool opensRight = reader.ReadBoolean();
                        newMap.Doors[new Point(x, y)] = new Door(mapId, x, y, false, opensRight);
                    }

                    //load warps
                    ushort warpCount = reader.ReadUInt16();
                    for (int i = 0; i < warpCount; i++)
                    {
                        byte sourceX = reader.ReadByte();
                        byte sourceY = reader.ReadByte();
                        ushort targetMapId = reader.ReadUInt16();
                        byte targetX = reader.ReadByte();
                        byte targetY = reader.ReadByte();
                        Warp warp = new Warp(sourceX, sourceY, targetX, targetY, mapId, targetMapId);
                        newMap.Exits[new Point(sourceX, sourceY)] = warp;
                    }

                    //load worldmaps for this map
                    byte wMapCount = reader.ReadByte();
                    for (int i = 0; i < wMapCount; i++)
                    {
                        byte x = reader.ReadByte();
                        byte y = reader.ReadByte();
                        uint CRC = reader.ReadUInt32();
                        if (WorldMaps.ContainsKey(CRC))
                            newMap.WorldMaps[new Point(x, y)] = WorldMaps[CRC];
                    }

                    //add the map to the map list
                    if (Maps.TryAdd(mapId, newMap))
                        newMap.SetData($@"{Paths.MapFiles}lod{newMap.Id}.map");
                }
            }
        }
        internal void Save()
        {
            MemoryStream cacheStream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(cacheStream))
            {
                writer.Write(1);

                //write world maps
                writer.Write((ushort)WorldMaps.Count);
                foreach (WorldMap worldMap in WorldMaps.Values)
                {
                    writer.Write(worldMap.Field);
                    writer.Write((byte)worldMap.Nodes.Count);
                    foreach (WorldMapNode worldMapNode in worldMap.Nodes)
                    {
                        writer.Write(worldMapNode.Position.X);
                        writer.Write(worldMapNode.Position.Y);
                        writer.Write(worldMapNode.Name);
                        writer.Write(worldMapNode.MapId);
                        writer.Write((byte)worldMapNode.Point.X);
                        writer.Write((byte)worldMapNode.Point.Y);
                    }
                }

                //write maps
                writer.Write((ushort)Maps.Count);
                foreach (Map map in Maps.Values)
                {
                    //write map info
                    writer.Write(map.Id);
                    writer.Write(map.SizeX);
                    writer.Write(map.SizeY);
                    writer.Write(map.Name);
                    writer.Write((uint)map.Flags);
                    writer.Write(map.Music);

                    //write doors
                    writer.Write((byte)map.Doors.Count);
                    foreach (Door door in map.Doors.Values)
                    {
                        writer.Write(door.Point.X);
                        writer.Write(door.Point.Y);
                        writer.Write(door.OpenRight);
                    }

                    //write warps
                    writer.Write((ushort)map.Exits.Count);
                    foreach (Warp warp in map.Exits.Values)
                    {
                        writer.Write(warp.SourceX);
                        writer.Write(warp.SourceY);
                        writer.Write(warp.TargetMapId);
                        writer.Write(warp.TargetX);
                        writer.Write(warp.TargetY);
                    }

                    //write worldmaps for this map
                    writer.Write((byte)map.WorldMaps.Count);
                    foreach (KeyValuePair<Point, WorldMap> keyValuePair in map.WorldMaps)
                    {
                        writer.Write((byte)keyValuePair.Key.X);
                        writer.Write((byte)keyValuePair.Key.Y);
                        writer.Write(keyValuePair.Value.GetCrc32());
                    }
                }

                Server.DataBase.Cache.Replace(MapKey, cacheStream.ToArray());
            }
        }

        /// <summary>
        /// Adds a single object to a map. Sends and sets all relevant data.
        /// </summary>
        /// <param name="vObject">Any visible object.</param>
        /// <param name="location">The map and point you want to add it to.</param>
        internal void AddObjectToMap(VisibleObject vObject, Location location)
        {
            if (vObject == null) return;

            lock(Maps[location.MapId].Objects)
            {
                //change location of the object and add it to the map
                vObject.Map = Maps[location.MapId];
                vObject.Point = location.Point;
                Maps[location.MapId].Objects.AddOrUpdate(vObject.Id, vObject as WorldObject, (key, oldValue) => vObject);

                List<VisibleObject> itemMonsterToSend = new List<VisibleObject>();
                List<User> usersToSend = new List<User>();

                //get all objects that would be visible to this object and sort them
                foreach(var obj in ObjectsVisibleFrom(vObject))
                    if(obj is User)
                        usersToSend.Add(obj as User);
                    else
                        itemMonsterToSend.Add(obj);

                //if this object is a user
                if(vObject is User)
                {
                    User user = vObject as User;
                    
                    user.Client.Enqueue(Server.Packets.MapChangePending());     //pending map change
                    user.Client.Enqueue(Server.Packets.MapInfo(user.Map));      //send map info
                    user.Client.Enqueue(Server.Packets.Location(user.Point));   //send location

                    foreach (User u2s in usersToSend)
                    {   
                        user.Client.Enqueue(Server.Packets.DisplayUser(u2s));   //send it all the users
                        u2s.Client.Enqueue(Server.Packets.DisplayUser(user));   //send all the users this user as well
                    }

                    user.Client.Enqueue(Server.Packets.DisplayItemMonster(itemMonsterToSend.ToArray()));    //send it all the items, monsters, and merchants
                    user.Client.Enqueue(Server.Packets.Door(DoorsVisibleFrom(user).ToArray()));     //send the user all nearby doors
                    user.Client.Enqueue(Server.Packets.MapChangeComplete());    //send it mapchangecomplete
                    user.Client.Enqueue(Server.Packets.MapLoadComplete());      //send it maploadcomplete
                    user.Client.Enqueue(Server.Packets.DisplayUser(user));      //send it itself
                }
                else //if this object isnt a user
                    foreach (User u2s in usersToSend)
                        u2s.Client.Enqueue(Server.Packets.DisplayItemMonster(vObject)); //send all the visible users this object
            }
        }

        /// <summary>
        /// Adds many objects to the map. NON-USERS ONLY!
        /// </summary>
        /// <param name="vObjects">Any non-user visibleobject</param>
        /// <param name="location">The map and point you want to add it to.</param>
        internal void AddObjectsToMap(List<VisibleObject> vObjects, Location location)
        {
            if (vObjects.Count == 0) return;

            lock(Maps[location.MapId].Objects)
            {
                //change location of each object and add each item to the map
                foreach(VisibleObject vObj in vObjects)
                {
                    vObj.Map = Maps[location.MapId];
                    vObj.Point = location.Point;
                    Maps[location.MapId].Objects.AddOrUpdate(vObj.Id, vObj as WorldObject, (key, oldValue) => vObj);
                }

                //send all the visible users these objects
                foreach (User user in ObjectsVisibleFrom(vObjects[0]).OfType<User>())
                    user.Client.Enqueue(Server.Packets.DisplayItemMonster(vObjects.ToArray()));
            }
        }

        /// <summary>
        /// Removes a single object from the map.
        /// </summary>
        /// <param name="vObject">Any visible object you want removed.</param>
        internal void RemoveObjectFromMap(VisibleObject vObject)
        {
            if (vObject == null) return;

            lock(vObject.Map.Objects)
            {
                WorldObject w;
                if (vObject.Map.Objects.TryRemove(vObject.Id, out w))
                {
                    foreach (User user in ObjectsVisibleFrom(vObject).OfType<User>())
                        user.Client.Enqueue(Server.Packets.RemoveObject(vObject));

                    vObject.Map = null;
                }
            }
        }

        /// <summary>
        /// Moves a user from one map to another
        /// </summary>
        /// <param name="user"></param>
        internal void WarpUser(User user, Warp warp)
        {
            if(warp.Location == user.Location && Maps.ContainsKey(warp.TargetMapId))
            {
                if(Maps[warp.TargetMapId].IsWall(warp.TargetX, warp.TargetY))
                {
                    int dist = int.MaxValue;
                    Point nearestPoint = new Point(ushort.MaxValue, ushort.MaxValue);
                    Maps[warp.TargetMapId].Tiles.Keys.ToList().ForEach(t =>
                    {
                        Point tPoint = new Point(t.X, t.Y);
                        if (!Maps[warp.TargetMapId].IsWall(t.X, t.Y) && warp.TargetPoint.Distance(tPoint) < dist)
                        {
                            dist = warp.TargetPoint.Distance(tPoint);
                            nearestPoint = tPoint;
                        }
                    });
                    warp = new Warp(warp.Location, new Location(warp.TargetMapId, nearestPoint));
                }
                RemoveObjectFromMap(user);
                AddObjectToMap(user, warp.TargetLocation);
            }
        }

        /// <summary>
        /// Gets all objects the given object can see
        /// </summary>
        /// <param name="vObject">Object to base from.</param>
        /// <returns></returns>
        internal List<VisibleObject> ObjectsVisibleFrom(VisibleObject vObject, byte distance = 0)
        {
            lock (vObject.Map.Objects)
            {
                if (distance == 0)
                    return Maps[vObject.Location.MapId].Objects.Values.OfType<VisibleObject>().Where(obj => obj.WithinRange(vObject.Point) && vObject != obj).ToList();
                else
                    return Maps[vObject.Location.MapId].Objects.Values.OfType<VisibleObject>().Where(obj => obj.Point.Distance(vObject.Point) <= distance && vObject != obj).ToList();
            }
        }

        /// <summary>
        /// Gets all doors visible from the user.
        /// </summary>
        /// <param name="user">The user to base from.</param>
        /// <returns></returns>
        internal List<Door> DoorsVisibleFrom(User user) => user.Map.Doors.Values.Where(door => user.WithinRange(door.Point)).ToList();

        /// <summary>
        /// Resends all the current information for the given user.
        /// </summary>
        /// <param name="user">The user to refresh.</param>
        internal void Refresh(User user)
        {
            lock(user.Map.Objects)
            {
                user.Client.Enqueue(Server.Packets.MapInfo(user.Map));
                user.Client.Enqueue(Server.Packets.Location(user.Point));

                List<VisibleObject> itemMonsterToSend = new List<VisibleObject>();

                //get all objects that would be visible to this object and sort them
                foreach (VisibleObject obj in ObjectsVisibleFrom(user))
                    if (obj is User)
                    {
                        user.Client.Enqueue(Server.Packets.DisplayUser(obj as User));
                        (obj as User).Client.Enqueue(Server.Packets.RemoveObject(user));
                        (obj as User).Client.Enqueue(Server.Packets.DisplayUser(user));
                    }
                    else
                        itemMonsterToSend.Add(obj);

                user.Client.Enqueue(Server.Packets.DisplayItemMonster(itemMonsterToSend.ToArray()));
                user.Client.Enqueue(Server.Packets.MapLoadComplete());
                user.Client.Enqueue(Server.Packets.DisplayUser(user));
                user.Client.Enqueue(Server.Packets.RefreshResponse());
            }
        }

        /// <summary>
        /// Attempts to retreive a user by searching through the maps for the given name.
        /// </summary>
        /// <param name="name">The name of the user to search for.</param>
        /// <returns></returns>
        internal User TryGetUser(string name)
        {
            foreach (Map map in Maps.Values)
                lock (map.Objects)
                    foreach (User user in map.Objects.Values.OfType<User>())
                        if (user.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                            return user;

            return null;
        }

        /// <summary>
        /// Attempts to retreive an object by searching through the maps for a given Id.
        /// </summary>
        /// <param name="id">Id to search for.</param>
        /// <returns></returns>
        internal VisibleObject TryGetObject(int id, Map mapToTry = null)
        {
            if (mapToTry != null)
            {
                lock (mapToTry.Objects)
                    if (mapToTry.Objects.ContainsKey(id))
                        return mapToTry.Objects[id] as VisibleObject;
            }
            else
            {
                foreach (Map map in Maps.Values)
                    lock (map.Objects)
                        if (map.Objects.ContainsKey(id))
                            return map.Objects[id] as VisibleObject;
            }

            return null;
        }

        /// <summary>
        /// Attempts to retreive an object by searching through the maps at a given point.
        /// </summary>
        /// <param name="id">Point to check.</param>
        /// <returns></returns>
        internal MapObject TryGetObject(Point point, Map mapToTry = null)
        {
            if (mapToTry != null)
            {
                lock (mapToTry.Objects)
                {
                    if (mapToTry.Doors.ContainsKey(point))
                        return mapToTry.Doors[point];

                    //signposts
                    //boards
                    //items
                }
            }
            else
            {
                foreach (Map map in Maps.Values)
                    lock (map.Objects)
                    {
                        if (map.Doors.ContainsKey(point))
                            return map.Doors[point];

                        //signposts
                        //boards
                    }
            }

            return null;
        }

        /// <summary>
        /// Attempts to retreive the item at the specified point.
        /// </summary>
        /// <param name="point">Point of the item to retreive.</param>
        /// <param name="mapToTry">Map to try retreiving from.</param>
        /// <returns></returns>
        internal GroundItem TryGetItem(Point point, Map mapToTry = null)
        {
            if (mapToTry != null)
            {
                lock (mapToTry.Objects)
                {
                    GroundItem item = mapToTry.Objects.Values.OfType<GroundItem>().FirstOrDefault(i => i.Point == point);
                    if (item != null)
                        return item;
                }
            }
            else
            {
                foreach (Map map in Maps.Values)
                {
                    lock (map.Objects)
                    {
                        GroundItem item = map.Objects.Values.OfType<GroundItem>().FirstOrDefault(i => i.Point == point);
                        if (item != null)
                            return item;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates an amount of items with the given information.
        /// </summary>
        /// <param name="sprite">Visible sprite of the item.</param>
        /// <param name="color">Color of the sprite.</param>
        /// <param name="name">Name of the item.</param>
        /// <param name="count">Number of the item to create.</param>
        /// <param name="stackable">If the item is stackable</param>
        /// <returns></returns>
        internal List<Item> CreateItems(ushort sprite, byte color, string name, int count, bool stackable)
        {
            sprite += 32768;
            List<Item> items = new List<Item>();
            if (!stackable && count > 1)
                for (int i = 0; i < count; i++)
                {
                    Item newItem = new Item(0, sprite, color, name, 1, stackable, 500000, 500000, new TimeSpan());
                    items.Add(newItem);
                }
            else
            {
                Item newItem = new Item(0, sprite, color, name, count, stackable, 500000, 500000, new TimeSpan());
                items.Add(newItem);
            }

            return items;
        }

        /// <summary>
        /// Cleans up active world info of the user.
        /// </summary>
        /// <param name="user">The user to clean up</param>
        internal void ScrubUser(User user)
        {
            user.Save();
            RemoveObjectFromMap(user);
            user.Group?.TryRemove(user.Id);

            //remove from other things
        }
    }
}