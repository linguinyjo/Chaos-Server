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
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public VorlofScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = TrainingQuestHelper.GetQuestStatus(source);
        if (questStatus is TrainingQuestStatus.SpokenToRiona)
        {
            Subject.AddOption("Riona sent me", "vorlof_training_quest");
        } else if (TrainingQuestHelper.GetQuestStatus(source) is TrainingQuestStatus.CompletedDarsRequest)
        {
            Subject.AddOption("That was easy!", "vorlof_completed_dars_request");
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
