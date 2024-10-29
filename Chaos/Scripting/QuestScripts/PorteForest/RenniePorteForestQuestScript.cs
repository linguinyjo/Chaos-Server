using Chaos.Collections;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class RenniePorteForestQuestScript: DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public RenniePorteForestQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = PorteForestQuestHelper.GetQuestStatus(source);
        if (questStatus != PorteForestQuestStatus.KilledTheMantis) return;
        // give the ring
        var item = ItemFactory.Create("tristarRing");
        var ringGiven = source.Inventory.TryAddToNextSlot(item);
        if (ringGiven)
        {
            source.Inventory.TryGetRemoveByTemplateKey("turucPendant", out _);
            PorteForestQuestHelper.CompleteQuest(source);
            var mapInstance = SimpleCache.Get<MapInstance>("porteForestShop");
            var destination = new Location("porteForestShop",16, 16);
            source.TraverseMap(mapInstance, destination);
            Dialog.Close(source);
        }
        else
        {
            Dialog.Reply(source, "You can't carry anything else", "close");
        }
    }

    public override void OnDisplayed(Aisling source)
    { }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    { }
}
