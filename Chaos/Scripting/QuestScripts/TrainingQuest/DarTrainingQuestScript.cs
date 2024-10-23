using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class DarTrainingQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public DarTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        if (trainingQuestStatus is not TrainingQuestStatus.SpokenToDar) return;
        // already on this part of the quest, so check to see if player has mould
        var hasMould = source.Inventory.HasCount("mold", 3);
        if (hasMould)
        {
            var newDialog = new Dialog(
                Dialog.DialogSource,
                DialogFactory,
                ChaosDialogType.Normal,
                "Well, well... You've actually managed to collect the mold. I must admit, I'm mildly impressed. Perhaps there's a flicker of potential in you after all. Go back and tell Vorlof that you have succeeded in doing as I have asked.")
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
            source.Inventory.RemoveQuantity("mold", 3);
            source.GiveExperience(150);
            TrainingQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = new Dialog(
                Dialog.DialogSource,
                DialogFactory,
                ChaosDialogType.Normal,
                "Where's my mold? Don't tell me the rats are too formidable for your meager skills? Return when you've mustered the courage to complete this simple task. Until then, you're wasting my time.")
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
