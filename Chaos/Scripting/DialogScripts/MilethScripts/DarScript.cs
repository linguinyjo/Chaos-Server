using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class DarScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public DarScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        if (trainingQuestStatus is TrainingQuestStatus.SpokenToVorlof or TrainingQuestStatus.SpokenToDar)
        {
            AddTrainingQuestOption();
        } 
    }
    
    private void AddTrainingQuestOption()
    {
        Subject.AddOption("Training quest", "dar_training_quest");
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
