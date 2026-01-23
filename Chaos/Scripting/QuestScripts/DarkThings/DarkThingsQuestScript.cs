using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.DarkThings;

public class DarkThingsQuestScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public DarkThingsQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        switch (Subject.Template.TemplateKey)
        {
            case "dar_dark_things_initial":
                HandleInitialTemplate(source);
                break;
            case "dar_dark_things_accepted":
                HandleAcceptedTemplate(source);
                break;
            case "dar_dark_things_got_what_you_need":
                HandleGotWhatYouNeedTemplate(source);
                break;
        }
    }

    private void HandleGotWhatYouNeedTemplate(Aisling source)
    {
        var questStatus = DarkThingsQuestHelper.GetQuestStatus(source);
        var itemDisplayName = DarkThingsQuestHelper.GetItemDisplayName(questStatus);
        var hasTheItem = DarkThingsQuestHelper.HasRequiredItem(source, questStatus);
        if (hasTheItem)
        {
            DarkThingsQuestHelper.RemoveRequiredItem(source, questStatus);
            DarkThingsQuestHelper.CompleteQuest(source);
            var finishQuestDialog = CreateDialog(
                text:
                $"Excellent! This {itemDisplayName} is perfect for my studies. Come back to me soon and I will have another task for you.",
                nextKey: "Close"
            );
            finishQuestDialog.Display(source);
            return;
        }

        // Player does not have the item
        var reminderDialog = CreateDialog(
            text:
            $"It looks like you haven't brought me the {itemDisplayName}. Don't waste my time, Aisling. Return with what I need or don't return at all.",
            nextKey: "Close"
        );
        reminderDialog.Display(source);
    }

    private void HandleAcceptedTemplate(Aisling source)
    {
        if (DarkThingsQuestHelper.IsQuestAvailable(source))
        {
            var requiredItem = DarkThingsQuestHelper.StartQuest(source);
            var startQuestDialog = CreateDialog(
                text:
                $"Excellent! Bring me a {requiredItem} from the crypt. And don't keep me waiting, Aisling.",
                nextKey: "Close"
            );
            startQuestDialog.Display(source);
            return;
        }

        var notEligibleDialog = CreateDialog(
            text: "You are not allowed to accept this quest.",
            nextKey: "Close"
        );
        notEligibleDialog.Display(source);
    }


    private void HandleInitialTemplate(Aisling source)
    {
        var questStatus = DarkThingsQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case DarkThingsQuestStatus.None:
                if (DarkThingsQuestHelper.IsQuestAvailable(source))
                {
                    Subject.AddOption("I will", "dar_dark_things_accepted");
                    break;
                }

                var blockedQuestDialog = CreateDialog(
                    text:
                    "Oh you've already brought me something recently. Once I have finished studying it I will surely need more. Return to me soon.",
                    nextKey: "Close"
                );
                blockedQuestDialog.Display(source);
                break;
            case DarkThingsQuestStatus.FetchSpidersEye:
            case DarkThingsQuestStatus.FetchCentipedeGland:
            case DarkThingsQuestStatus.FetchSpidersSilk:
            case DarkThingsQuestStatus.FetchBatWing:
            case DarkThingsQuestStatus.FetchScorpionSting:
                Subject.AddOption("I've got what you need", "dar_dark_things_got_what_you_need");
                break;
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
            NextDialogKey = nextKey,
            Options = []
        };
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}