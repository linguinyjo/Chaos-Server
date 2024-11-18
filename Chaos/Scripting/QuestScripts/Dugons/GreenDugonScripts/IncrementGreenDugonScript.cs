using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class IncrementGreenDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper GreenDugonQuestHelper = new();

    /// <inheritdoc />
    public IncrementGreenDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}

    public override void OnDisplayed(Aisling source)
    {
       GreenDugonQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
