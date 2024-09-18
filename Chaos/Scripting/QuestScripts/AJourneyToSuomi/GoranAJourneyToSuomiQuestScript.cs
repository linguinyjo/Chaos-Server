using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.AJourneyToSuomi;

public class GoranAJourneyToSuomiQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;
    private readonly Dialog Dialog;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    /// <inheritdoc />
    public GoranAJourneyToSuomiQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var status = AJourneyToSuomiQuestHelper.GetQuestStatus(source);
        switch (status)
        {
            case AJourneyToSuomiQuestStatus.FetchFruitShipment:
            {
                var newDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "Ah, Dona's fruit shipment! Yes, yes, it's all packed and ready to go. I was wondering when she'd send someone. Those Mileth fruit pies won't bake themselves, will they? Here you go,  handle it with care, mind you. Some of those fruits bruise easier than a Peasant's ego!"
                )
                {
                    NextDialogKey = "Close"
                };
                newDialog.Display(source);
                var shipment = ItemFactory.Create("shipmentOfFruit");
                var itemAdded = source.Inventory.TryAddToNextSlot(shipment);
                if (itemAdded) AJourneyToSuomiQuestHelper.IncrementQuestStage(source);
                break;
            }
            case AJourneyToSuomiQuestStatus.ReceivedFruitShipment:
                var receivedFruitDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "I've given you the fruit already. Go and deliver it to Oona, I'm sure she needs it as soon as possible."
                )
                {
                    NextDialogKey = "Close"
                };
                receivedFruitDialog.Display(source);
                break;
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
