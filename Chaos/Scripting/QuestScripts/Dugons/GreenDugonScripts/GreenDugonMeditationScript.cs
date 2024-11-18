using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class GreenDugonMeditationScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper GreenDugonQuestHelper = new();

    /// <inheritdoc />
    public GreenDugonMeditationScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source) {}
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (optionIndex is 3)
        {
            Subject.Reply(
                source,
                "*The waters shimmer with approval, and you hear a resounding voice from students past*\n\"Sabonim, I understand the path of growth\"",
                "close"
            );
            GreenDugonQuestHelper.HandleMeditationSuccess(source);
        }
        else
        {
            Subject.Reply(
                source,
                "Nothing happens and you feel a rising sense of doubt...",
                "close"
            );
            GreenDugonQuestHelper.HandleMeditationFailed(source);
        }
    }
}
