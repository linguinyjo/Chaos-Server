using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class WhiteDugonMeditationScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public WhiteDugonMeditationScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (optionIndex is 1)
        {
            Subject.Reply(
                source,
                "*The waters shimmer with approval, and you hear a resounding voice from within.*\n\"Sabonim I understand my inner potential\"",
                "close"
            );
            WhiteDugonQuestHelper.HandleMeditationSuccess(source);
        }
        else
        {
            Subject.Reply(
                source,
                "Nothing happens and you feel a rising sense of doubt...",
                "close"
            );
            WhiteDugonQuestHelper.HandleMeditationFailed(source);
        }
    }
}
