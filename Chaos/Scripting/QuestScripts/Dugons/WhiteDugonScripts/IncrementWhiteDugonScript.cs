using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class IncrementWhiteDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly WhiteDugonQuestHelper WhiteDugonQuestHelper = new();
    
    /// <inheritdoc />
    public IncrementWhiteDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}

    public override void OnDisplayed(Aisling source)
    {
        WhiteDugonQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
