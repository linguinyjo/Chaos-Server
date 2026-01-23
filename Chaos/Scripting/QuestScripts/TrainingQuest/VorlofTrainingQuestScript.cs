using Chaos.DarkAges.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class VorlofTrainingQuestScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public VorlofTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    #region ScriptVars

    protected byte Class { get; init; }

    #endregion

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
    }

    public override void OnDisplayed(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        switch (trainingQuestStatus)
        {
            case TrainingQuestStatus.SpokenToRiona:
            {
                TrainingQuestHelper.IncrementQuestStage(source);
                break;
            }
            case TrainingQuestStatus.CompletedTorrencesRequest:
            {
                TrainingQuestHelper.CompleteQuest(source);

                break;
            }
        }
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}