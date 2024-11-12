using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public class DojoWarpScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public DojoWarpScript(Dialog subject, IDialogFactory dialogFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var currentDugon = source.Trackers.Enums.TryGetValue<Dugon>(out var status) ? status : Dugon.None;
        Subject.AddOption("None", "Close");
        if(currentDugon == Dugon.None) return;
        var availableDugons = Enum.GetValues(typeof(Dugon))
            .Cast<Dugon>()
            .Where(d => d <= currentDugon)
            .ToList();
        
        foreach (var dugon in availableDugons)
        {
            switch (dugon)
            {
                case Dugon.White:
                    Subject.AddOption("White Dugon", "");
                    break;
                case Dugon.Green:
                    Subject.AddOption("Green Dugon", "");
                    break;
                case Dugon.Blue:
                    Subject.AddOption("Blue Dugon", "");
                    break;
                case Dugon.Yellow:
                    Subject.AddOption("Yellow Dugon", "");
                    break;
                case Dugon.Purple:
                    Subject.AddOption("Purple Dugon", "");
                    break;
                case Dugon.Brown:
                    Subject.AddOption("Brown Dugon", "");
                    break;
                case Dugon.Red:
                    Subject.AddOption("Red Dugon", "");
                    break;
                case Dugon.Black:
                    Subject.AddOption("Black Dugon", "");
                    break;
            }
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var mapKey = GetMapKey(optionIndex);
        if (mapKey == null) return;
        var mapInstance = SimpleCache.Get<MapInstance>(mapKey);
        var destination = new Location(mapKey,6, 4);
        source.TraverseMap(mapInstance, destination);
        Subject.Close(source);
    }

    private static string? GetMapKey(byte? optionIndex = null)
    {
        return optionIndex switch
        {
            2 => "whiteGrove",
            3 => "greenGrove",
            4 => "blueGrove",
            5 => "yellowGrove",
            6 => "purpleGrove",
            7 => "brownGrove",
            8 => "redGrove",
            9 => "blackGrove",
            _ => null
        };
    }
}
