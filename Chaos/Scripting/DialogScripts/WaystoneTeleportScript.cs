using Chaos.Collections;
using Chaos.Models.Data;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.SpellScripts.WaystoneScripts;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class WaystoneTeleportScript: DialogScriptBase
{
    private readonly ISimpleCache SimpleCache;
    
    /// <inheritdoc />
    public WaystoneTeleportScript(Dialog subject, ISimpleCache simpleCache)
        : base(subject)   {
        SimpleCache = simpleCache;
    }
    
    private readonly List<Waystone> _optionWaystones = [];

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (source.Trackers.Flags.TryGetFlag<Waystone>(out var visitedWaystones))
        {
            // Iterate over all possible Waystone values
            foreach (Waystone value in Enum.GetValues(typeof(Waystone)))
            {
                if (value == Waystone.None) continue;
                // Check if this specific waystone is visited
                if (!visitedWaystones.HasFlag(value)) continue;
                Subject.AddOption(value.ToString(), "");
                _optionWaystones.Add(value);
            }
        }
        else
        {
            Console.WriteLine("No waystones visited yet.");
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (!optionIndex.HasValue) return;

        var adjustedOptionIndex = optionIndex.Value - 1;
        if (adjustedOptionIndex >= _optionWaystones.Count)
        {
            Console.WriteLine($"Invalid option index: {optionIndex}, max: {_optionWaystones.Count - 1}");
            return;
        }

        var selectedWaystone = _optionWaystones[adjustedOptionIndex];

        if (selectedWaystone != Waystone.None)
        {
            var waystoneInfo = WaystoneData.GetWaystoneInfo(selectedWaystone);
            Console.WriteLine($"Teleporting to {selectedWaystone}");
            
            var mapInstance = SimpleCache.Get<MapInstance>(waystoneInfo.MapName);
            var destination = waystoneInfo.Location;
            
            if (source.Group == null)
            {
                _ = PerformTeleport(source, mapInstance, destination);
                Subject.Close(source);
                return;
            }

            _ = PerformTeleportAsync(source.Group, mapInstance, destination);
            
            Subject.Close(source);
        }
        else
        {
            Console.WriteLine("No valid waystone selected.");
        }
    }
    
    private static async Task PerformTeleportAsync(IEnumerable<Aisling> aislings, MapInstance mapInstance, Location destination)
    {
        var enumerable = aislings as Aisling[] ?? aislings.ToArray();
        var currentLocations = enumerable.Select(aisling => aisling.GetCurrentLocation()).ToList();
        foreach (var aisling in enumerable)
        {
            aisling.Client.SendSound(47, false);
            var anim = new Animation
            {
                AnimationSpeed = 150,
                TargetAnimation = 78
            };
            foreach (var location in currentLocations)
            {
                aisling.Client.SendAnimation(anim.GetPointAnimation(location));
            }
        }

        await Task.Delay(2000);

        foreach (var aisling in enumerable)
        {
            aisling.TraverseMap(mapInstance, destination);
        }
    }

    private static async Task PerformTeleport(Aisling aisling, MapInstance mapInstance, Location destination)
    {
        aisling.Client.SendSound(47, false);
        var anim = new Animation
        {
            AnimationSpeed = 150,
            TargetAnimation = 78
        };
        aisling.Client.SendAnimation(anim.GetPointAnimation(aisling.GetCurrentLocation()));
        await Task.Delay(2000);
        aisling.TraverseMap(mapInstance, destination);
    }
}
