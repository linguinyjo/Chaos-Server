using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.SpareAStick;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class CalloughScript : DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public CalloughScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    #region ScriptVars

    protected byte Class { get; init; }

    #endregion

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = SpareAStickQuestHelper.GetQuestStatus(source);
        if (questStatus == SpareAStickQuestStatus.Completed) return;
        Subject.AddOption("Spare a stick", "callough_spare_a_stick_a");
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}