using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class WhiteDugonQuestHelper : BaseQuestHelper<WhiteDugonQuestStatus>
{
    protected override string QuestId => "WhiteDugon";
    protected override Dugon Dugon => Dugon.White;
    protected override string QuestBlockId => "WhiteDugonBlock";
    protected override string QuestName => "White";
    protected override int RequiredLevel => 11;
    protected override WhiteDugonQuestStatus CompletedStatus => WhiteDugonQuestStatus.Completed;
    protected override WhiteDugonQuestStatus NoneStatus => WhiteDugonQuestStatus.None;
    protected override TimeSpan AllowedTime => TimeSpan.FromMinutes(15);

    protected override WhiteDugonQuestStatus GetNextStatus(WhiteDugonQuestStatus currentStatus)
    {
        return currentStatus switch
        {
            WhiteDugonQuestStatus.None => WhiteDugonQuestStatus.Started,
            WhiteDugonQuestStatus.Started => WhiteDugonQuestStatus.KilledBat,
            WhiteDugonQuestStatus.KilledBat => WhiteDugonQuestStatus.ReturnedToSabonim,
            WhiteDugonQuestStatus.ReturnedToSabonim => WhiteDugonQuestStatus.MeditationCompleted,
            WhiteDugonQuestStatus.MeditationCompleted => WhiteDugonQuestStatus.Completed,
            _ => currentStatus
        };
    }
}

public enum WhiteDugonQuestStatus
{
    None = 0,
    Started = 1,
    KilledBat = 2,
    ReturnedToSabonim = 3,
    MeditationCompleted = 4,
    Completed = 5
}