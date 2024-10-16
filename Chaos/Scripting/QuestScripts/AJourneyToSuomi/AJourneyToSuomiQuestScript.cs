using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.AJourneyToSuomi;

public class AJourneyToSuomiQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public AJourneyToSuomiQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = AJourneyToSuomiQuestHelper.GetQuestStatus(source);
        if (questStatus is AJourneyToSuomiQuestStatus.FetchFruitShipment or AJourneyToSuomiQuestStatus.ReceivedFruitShipment)
        {
            QuestHandler(source);
        } 
    }

    private void QuestHandler(Aisling source)
    {
        var hasShipment = source.Inventory.HasCountByTemplateKey("shipmentOfFruit", 1);
        if (hasShipment)
        {
            var newDialog = CreateDialog(
                text: "Oh, you're back! And with the fruit! You're a lifesaver, truly. Now I can finally get back to baking. Here's a little something for your trouble.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("shipmentOfFruit", 1);
            AJourneyToSuomiQuestHelper.CompleteQuest(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "I see you've returned, but without my shipment. Please do hurry. I can't begin service until I receive my weekly supply of fruit.",
                nextKey: "Close"
            );
            newDialog.Display(source);
        }
    }

    private Dialog CreateDialog(string text, string nextKey)
    {
        return new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            text)
        {
            NextDialogKey = nextKey
        };
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = AJourneyToSuomiQuestHelper.GetQuestStatus(source);
        if (optionIndex == 1 && questStatus is AJourneyToSuomiQuestStatus.None)
        {
            AJourneyToSuomiQuestHelper.StartQuest(source);
        }
    }
}
