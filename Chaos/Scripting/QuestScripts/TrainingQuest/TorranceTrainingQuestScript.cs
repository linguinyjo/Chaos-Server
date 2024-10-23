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
    private readonly Dialog Dialog;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    /// <inheritdoc />
    public TorranceTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        switch (trainingQuestStatus)
        {
            case TrainingQuestStatus.FromDarToVorlof:
                TrainingQuestHelper.IncrementQuestStage(source);
                break;
            case TrainingQuestStatus.SpokenToTorrance:
            {
                var hasGlands = source.Inventory.HasCountByTemplateKey("vipersGland", 1);
                if (hasGlands)
                {
                    var newDialog = new Dialog(
                        Dialog.DialogSource,
                        DialogFactory,
                        ChaosDialogType.Normal,
                        "Well, I'll be... You actually managed to get them. You've done well, I suppose. Here's a bit of coin for your trouble. Now, don't let it go to your head – there's still much for you to learn. Go back and Speak with Vorlof.")
                    {
                        NextDialogKey = "Close"
                    };
                    newDialog.Display(source);
                    source.Inventory.RemoveQuantityByTemplateKey("vipersGland", 1);
                    source.GiveExperience(150);
                    source.TryGiveGold(500);
                    TrainingQuestHelper.IncrementQuestStage(source);
                }
                else
                {
                    var newDialog = new Dialog(
                        Dialog.DialogSource,
                        DialogFactory,
                        ChaosDialogType.Normal,
                        "Back already? And empty-handed, no less. What's the matter, couldn't handle a few little vipers? Stop wasting my time and get back out there. And this time, don't come back until you have something to show for your efforts.")
                    {
                        NextDialogKey = "Close"
                    };
                    newDialog.Display(source);
                }
                break;
            }
        }
    }

    public override void OnDisplayed(Aisling source)
    {
       
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
