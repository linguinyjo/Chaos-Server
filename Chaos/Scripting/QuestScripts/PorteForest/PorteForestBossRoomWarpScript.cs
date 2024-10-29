using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class PorteForestBossRoomWarpScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public PorteForestBossRoomWarpScript(ReactorTile subject, IDialogFactory dialogFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        SimpleCache = simpleCache;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        var questStatus = PorteForestQuestHelper.GetQuestStatus(aisling);
        if (questStatus == PorteForestQuestStatus.Completed) return;
        var hasPendant = aisling.Inventory.HasCountByTemplateKey("turucPendant", 1);
        if (!hasPendant)
        {
            DisplayDialog(aisling);
            return;
        }
        var mapInstance = SimpleCache.Get<MapInstance>("porteForestPeak");
        var destination = new Location("porteForestPeak",9, 18);
        aisling.TraverseMap(mapInstance, destination);
    }
    
    private void DisplayDialog(Aisling source)
    {
        var newDialog = new Dialog(
            source,
            DialogFactory,
            ChaosDialogType.Normal,
            "An unknown force blocks you from entering.")
        {
            NextDialogKey = "close"
        };
        newDialog.Display(source);
    }
}