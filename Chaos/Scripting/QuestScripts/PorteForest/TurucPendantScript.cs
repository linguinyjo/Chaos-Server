using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class TurucPendantScript : ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public TurucPendantScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;

        var questStatus = PorteForestQuestHelper.GetQuestStatus(aisling);
        if (questStatus != PorteForestQuestStatus.DeliveredTheRoots) return;
        DisplayDialog(aisling);

        var item = ItemFactory.Create("turucPendant");
        var pendantAcquired = aisling.Inventory.TryAddToNextSlot(item);
        if (pendantAcquired)
        {
            source.Trackers.Enums.Set(PorteForestQuestStatus.FoundThePendant);
        }
    }

    private void DisplayDialog(Aisling source)
    {
        var newDialog = new Dialog(
            source,
            DialogFactory,
            ChaosDialogType.Normal,
            "You notice something shimmering in the corner of your eye. As you approach, you discover an ancient pendant in amongst the flowers. The Turuc Pendant gleams with a faint blue light, its metallic surface etched with intricate spiraling patterns.")
        {
            NextDialogKey = "close"
        };
        newDialog.Display(source);
    }
}