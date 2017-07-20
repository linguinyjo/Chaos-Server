﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Chaos.Objects;
using Newtonsoft.Json;

namespace Chaos
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class Map
    {
        internal ConcurrentDictionary<int, WorldObject> Objects { get; set; }
        internal ConcurrentDictionary<Point, Door> Doors { get; set; }
        [JsonProperty]
        internal ushort Id { get; }
        internal byte SizeX { get; }
        internal byte SizeY { get; }
        internal byte[] Data { get; private set; }
        internal ushort CRC => CRC16.Calculate(Data);
        internal Dictionary<Point, Tile> Tiles { get; }
        internal MapFlags Flags { get; set; }
        internal string Name { get; set; }
        internal sbyte Music { get; set; }
        internal Dictionary<Point, Warp> Exits { get; set; }
        internal Dictionary<Point, WorldMap> WorldMaps { get; set; }
        internal Tile this[ushort x, ushort y] => Tiles[new Point(x, y)];
        internal Tile this[Point point] => Tiles[point];

        internal Map(ushort id, byte sizeX, byte sizeY, MapFlags flags, string name, sbyte music)
        {
            Id = id;
            SizeX = sizeX;
            SizeY = sizeY;
            Flags = flags;
            Name = name;
            Tiles = new Dictionary<Point, Tile>();
            Exits = new Dictionary<Point, Warp>();
            WorldMaps = new Dictionary<Point, WorldMap>();
            Objects = new ConcurrentDictionary<int, WorldObject>();
            Doors = new ConcurrentDictionary<Point, Door>();
        }

        [JsonConstructor]
        internal Map(ushort id)
        {
            Id = id;
        }

        internal void SetData(byte[] data)
        {
            Tiles.Clear();
            Data = data;
            int num = 0;
            for (ushort y = 0; y < SizeY; y++)
                for (ushort x = 0; x < SizeX; x++)
                {
                    Point key = new Point(x, y);
                    short background = (short)(data[num++] | data[num++] << 8);
                    short leftForeground = (short)(data[num++] | data[num++] << 8);
                    short rightForeground = (short)(data[num++] | data[num++] << 8);
                    Tiles[key] = new Tile(background, leftForeground, rightForeground);
                }
        }
        internal void SetData(string path)
        {
            if (File.Exists(path))
                SetData(File.ReadAllBytes(path));
        }

        internal bool HasFlag(MapFlags flag) => Flags.HasFlag(flag);

        internal bool IsWall(ushort x, ushort y) => x < 0 || y < 0 || x >= SizeX || y >= SizeY || this[x, y].IsWall;
        internal bool IsWall(Point p) => IsWall(p.X, p.Y);
    }
}