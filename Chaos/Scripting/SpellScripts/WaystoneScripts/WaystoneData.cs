using Chaos.Collections;
using Chaos.Models.World;

namespace Chaos.Scripting.SpellScripts.WaystoneScripts;

public static class WaystoneData
{
    // Struct for the data
    public struct WaystoneInfo(string mapName, Location location)
    {
        public string MapName = mapName;
        public Location Location = location;
    }

    // Static dictionary to map Waystone enum to its data
    private static readonly Dictionary<Waystone, WaystoneInfo> WaystoneDetails = new()
    {
        { Waystone.MilethWaystone, new WaystoneInfo("milethVillage", new Location("milethVilage", 97, 31)) },
        { Waystone.PietWaystone,new WaystoneInfo("pietVillage", new Location("pietVilage", 3, 4)) },
    };

    // Methods to access the data
    public static WaystoneInfo GetWaystoneInfo(Waystone waystone)
    {
        return WaystoneDetails[waystone];
    }
}

[Flags]
public enum Waystone
{
    None = 0,
    MilethWaystone = 1 << 0, // 1
    PietWaystone = 1 << 1,   // 2
    // Add more waystones as needed, e.g.:
    // AbelWaystone = 1 << 2, // 4
    // etc.
}
