using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class AoifeScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public AoifeScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isPeasant = source.UserStatSheet.BaseClass is BaseClass.Peasant;
        if (isPeasant)
        {
            HandlePeasantDisplay(source);
            return;
        }
        HandleNonPeasantDisplay();
    }

    private void HandleNonPeasantDisplay()
    {
        // Maybe add some options here, e.g., where to hunt
       
    }

    private void HandlePeasantDisplay(Aisling source)
    {
        if (source.Trackers.Enums.HasValue(TrainingQuestStatus.Completed))
        {
            Subject.AddOption("I am ready to choose my path", "aoife_choose_path");
        }
        else
        {
            Subject.AddOption("I want to choose my path", "aoife_choose_path_help");
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
