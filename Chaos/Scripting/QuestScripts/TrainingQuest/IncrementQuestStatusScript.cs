using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class IncrementQuestStatusScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public IncrementQuestStatusScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
    }

    public override void OnDisplayed(Aisling source)
    {
       TrainingQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
