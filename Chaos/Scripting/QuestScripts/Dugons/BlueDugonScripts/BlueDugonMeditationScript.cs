using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class BlueDugonMeditationScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly BlueDugonQuestHelper BlueDugonQuestHelper = new();

    /// <inheritdoc />
    public BlueDugonMeditationScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (optionIndex is 4)
        {
            Subject.Reply(
                source,
                "*The waters shimmer with approval, and you hear a resounding voice from students past*\n\"Sabonim, I understand how to flow like water\"",
                "close"
            );
            BlueDugonQuestHelper.HandleMeditationSuccess(source);
        }
        else
        {
            Subject.Reply(
                source,
                "Nothing happens and you feel a rising sense of doubt...",
                "close"
            );
            BlueDugonQuestHelper.HandleMeditationFailed(source);
        }
    }
}
