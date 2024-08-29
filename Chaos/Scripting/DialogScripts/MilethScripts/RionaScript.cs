using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class RionaScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public RionaScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (!source.HasClass(BaseClass.Peasant))
        {
            HandleNonPeasantDisplay();
            return;
        }

        HandlePeasantDisplay(source);
    }

    private void HandleNonPeasantDisplay()
    {
        // Maybe add some options here, e.g., where to hunt
        Console.WriteLine("Quest completed and already accepted path");
    }

    private void HandlePeasantDisplay(Aisling source)
    {
        if (TrainingQuestHelper.IsQuestAvailable(source))
        {
            AddTrainingQuestOption();
        }
    }

    private void AddTrainingQuestOption()
    {
        Subject.AddOption("Training quest", "riona_training_quest");
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
