using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class VorlofTrainingQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    /// <inheritdoc />
    public VorlofTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}

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
                TrainingQuestHelper.IncrementQuestStage(source);
                source.GiveExperience(250);
                var legendMark = new LegendMark(
                    "Completed Vorlof's training",
                    "TrainingQuest",
                    MarkIcon.Victory,
                    MarkColor.White,
                    1,
                    GameTime.Now);
                source.Legend.AddUnique(legendMark);
                break;
            }
        }
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
