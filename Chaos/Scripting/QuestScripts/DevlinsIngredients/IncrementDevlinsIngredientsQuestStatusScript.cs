using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.DevlinsIngredients;

public class IncrementDevlinsIngredientsQuestStatusScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public IncrementDevlinsIngredientsQuestStatusScript(Dialog subject, IDialogFactory dialogFactory)
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
       DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
