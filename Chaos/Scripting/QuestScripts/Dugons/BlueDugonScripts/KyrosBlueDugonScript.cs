using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class KyrosBlueDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly BlueDugonQuestHelper BlueDugonQuestHelper = new();

    /// <inheritdoc />
    public KyrosBlueDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = BlueDugonQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case BlueDugonQuestStatus.Started when BlueDugonQuestHelper.HasPlayerFailed(source):
                Subject.Reply(
                    source,
                    "You failed this task, young monk. Do not be disheartened. Return to me after you have contemplated your failure, and then maybe I will allow you to try again.",
                    "Close"
                );
                break;
            case BlueDugonQuestStatus.Started:
                Subject.Reply(
                    source,
                    "You still have time. Go out and kill a turtle and then return to me.",
                    "Close"
                );
                break;
            case BlueDugonQuestStatus.KilledTurtle:
                Subject.Reply(
                    source,
                    "Well done. You have proved yourself worthy in combat.",
                    "kyros_blue_dugon_meditation_1"
                );
                BlueDugonQuestHelper.IncrementQuestStage(source);
                break;
            case BlueDugonQuestStatus.ReturnedToSabonim:
                Subject.Reply(
                    source,
                    BlueDugonQuestHelper.IsPlayerBlocked(source)
                        ? "Hmmm, you did not find the wisdom required. Ponder on your failings for a while and then you may try again."
                        : "Go now and enter the Sapphire Stream. You must perform your meditation if you want to train in the martial arts.",
                    "close"
                );
                break;
            case BlueDugonQuestStatus.MeditationCompleted:
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
