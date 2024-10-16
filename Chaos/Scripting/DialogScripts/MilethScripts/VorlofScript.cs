using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class VorlofScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public VorlofScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = TrainingQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case TrainingQuestStatus.SpokenToRiona:
                Subject.AddOption("Riona sent me", "vorlof_training_quest");
                break;
            case TrainingQuestStatus.CompletedDarsRequest:
                Subject.AddOption("That was easy!", "vorlof_completed_dars_request");
                break;
            case TrainingQuestStatus.CompletedTorrencesRequest:
                Subject.AddOption("Training quest", "vorlof_completed_torrances_request");
                break;
            case TrainingQuestStatus.Completed:
            {
                var isPeasant = source.UserStatSheet.BaseClass is BaseClass.Peasant;
                Subject.AddOption("What should I do now?",
                    isPeasant ? "vorlof_help_for_peasant" : "vorlof_help_for_first_circle_a");
                break;
            }
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
