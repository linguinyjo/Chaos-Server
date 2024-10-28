using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class StartPorteForestQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public StartPorteForestQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = PorteForestQuestHelper.GetQuestStatus(source);
        if (questStatus == PorteForestQuestStatus.None)
        {
            PorteForestQuestHelper.StartQuest(source);
        }
    }
    
    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
