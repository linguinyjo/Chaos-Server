using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class KyrosWhiteDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public KyrosWhiteDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = WhiteDugonQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case WhiteDugonQuestStatus.Started when WhiteDugonQuestHelper.HasPlayerFailed(source):
                Subject.Reply(
                    source,
                    "You failed this simple task, young monk. Not many fall short at the very first hurdle. Return to me after you have contemplated your failure, and then maybe I will allow you to try again.",
                    "Close"
                );
                break;
            case WhiteDugonQuestStatus.Started:
                Subject.Reply(
                    source,
                    "You still have time. Go out and kill a bat and then return to me.",
                    "Close"
                );
                break;
            case WhiteDugonQuestStatus.KilledBat:
                Subject.Reply(
                    source,
                    "Well done. You have proved yourself worthy in combat.",
                    "kyros_white_dugon_meditation_1"
                );
                break;
            case WhiteDugonQuestStatus.ReturnedToSabonim:
                Subject.Reply(
                    source,
                    WhiteDugonQuestHelper.IsPlayerBlocked(source)
                        ? "Hmmm, you did not find the wisdom required. Ponder on your failings for a while and then you may try again."
                        : "Go now and enter the Sapphire Stream. You must perform your meditation if you want to train in the martial arts.",
                    "close"
                );
                break;
            case WhiteDugonQuestStatus.MeditationCompleted:
                // Handle this in the verbal script
                Subject.Reply(
                    source,
                    "Have you completed your meditation?",
                    "close"
                );
                break;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
