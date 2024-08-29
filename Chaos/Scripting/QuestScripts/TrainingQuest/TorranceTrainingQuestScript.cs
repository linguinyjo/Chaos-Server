using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class TorranceTrainingQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    /// <inheritdoc />
    public TorranceTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        if (trainingQuestStatus is TrainingQuestStatus.FromDarToVorlof)
        {
            TrainingQuestHelper.IncrementQuestStage(source);
        }
        else if (trainingQuestStatus is TrainingQuestStatus.SpokenToTorrance)
        {
            var hasGlands = source.Inventory.HasCount("something", 3);
            if (hasGlands)
            {
                var newDialog = new Dialog(
                    source,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "Well, I'll be... You actually managed to get them. You've done well, I suppose. Here's a bit of coin for your trouble. Now, don't let it go to your head – there's still much for you to learn. Speak with Vorlof again; I'm sure he'll be pleased to see you.")
                {
                    NextDialogKey = "Close"
                };
                newDialog.Display(source);
                TrainingQuestHelper.IncrementQuestStage(source);
            }
            else
            {
                var newDialog = new Dialog(
                    source,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "Back already? And empty-handed, no less. What's the matter, couldn't handle a few little vipers? Stop wasting my time and get back out there. And this time, don't come back until you have something to show for your efforts")
                {
                    NextDialogKey = "Close"
                };
                newDialog.Display(source);
            }
        }
    }

    public override void OnDisplayed(Aisling source)
    {
       
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
