using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorSlainScript : DialogScriptBase
{
    /// <inheritdoc />
    public TerrorSlainScript(Dialog subject) : base(subject)
    {
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        TerrorQuestHelper.CompleteQuest(source);
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}