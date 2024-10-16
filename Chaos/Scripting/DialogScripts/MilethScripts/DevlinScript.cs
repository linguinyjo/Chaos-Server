using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.DevlinsIngredients;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class DevlinScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public DevlinScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isAvailable = DevlinsIngredientsQuestHelper.IsQuestAvailable(source);
        if (isAvailable)
        {
            Subject.AddOption("Devlin's ingredients", "devlins_ingredients_quest_a");
        }
    }

    private void HandleNonPeasantDisplay()
    {
        // Maybe add some options here, e.g., where to hunt
        Console.WriteLine("Quest completed and already accepted path");
    }

    private void HandlePeasantDisplay(Aisling source)
    {
        
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
