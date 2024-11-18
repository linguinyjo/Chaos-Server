using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class KyrosGreenDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper GreenDugonQuestHelper = new();

    /// <inheritdoc />
    public KyrosGreenDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = GreenDugonQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GreenDugonQuestStatus.Started when GreenDugonQuestHelper.HasPlayerFailed(source):
                Subject.Reply(
                    source,
                    "You failed this task, young monk. Do not be disheartened. Return to me after you have contemplated your failure, and then maybe I will allow you to try again.",
                    "Close"
                );
                break;
            case GreenDugonQuestStatus.Started:
                Subject.Reply(
                    source,
                    "You still have time. Go out and kill a crab and then return to me.",
                    "Close"
                );
                break;
            case GreenDugonQuestStatus.KilledCrab:
                Subject.Reply(
                    source,
                    "Well done. You have proved yourself worthy in combat.",
                    "kyros_green_dugon_meditation_1"
                );
                break;
            case GreenDugonQuestStatus.ReturnedToSabonim:
                Subject.Reply(
                    source,
                    GreenDugonQuestHelper.IsPlayerBlocked(source)
                        ? "Hmmm, you did not find the wisdom required. Ponder on your failings for a while and then you may try again."
                        : "Go now and enter the Sapphire Stream. You must perform your meditation if you want to train in the martial arts.",
                    "close"
                );
                break;
            case GreenDugonQuestStatus.MeditationCompleted:
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
