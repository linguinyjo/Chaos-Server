using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class RionaTrainingQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public RionaTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var hasReceivedQuest = TrainingQuestHelper.GetQuestStatus(source);
        if (hasReceivedQuest is TrainingQuestStatus.Completed)
        {
            //Completed the quest already 
            var newDialog = new Dialog(
                source,
                DialogFactory,
                ChaosDialogType.CloseDialog,
                "You've already completed your training.")
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
        } else if (hasReceivedQuest is TrainingQuestStatus.None)
        {
            // not on the quest so start it
            TrainingQuestHelper.StartQuest(source);
        }
        else
        {
            //On the quest already 
            var newDialog = new Dialog(
                source,
                DialogFactory,
                ChaosDialogType.Normal,
                "I've done for you all I can. Go now and speak to Vorlof. He'll know what to do with you.")
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
