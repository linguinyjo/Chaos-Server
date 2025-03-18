using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class IncrementBlueDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper BlueDugonQuestHelper = new();

    /// <inheritdoc />
    public IncrementBlueDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}

    public override void OnDisplayed(Aisling source)
    {
        BlueDugonQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
