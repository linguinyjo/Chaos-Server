using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class BlueDugonQuestHelper : BaseQuestHelper<BlueDugonQuestStatus>
{
    protected override string QuestId => "BlueDugon";
    protected override Dugon Dugon => Dugon.Blue;
    protected override string QuestBlockId => "BlueDugonBlock";
    protected override string QuestName => "Blue";
    protected override int RequiredLevel => 33;
    protected override BlueDugonQuestStatus CompletedStatus => BlueDugonQuestStatus.Completed;
    protected override BlueDugonQuestStatus NoneStatus => BlueDugonQuestStatus.None;
    protected override TimeSpan AllowedTime => TimeSpan.FromMinutes(30);

    protected override BlueDugonQuestStatus GetNextStatus(BlueDugonQuestStatus currentStatus)
    {
        return currentStatus switch
        {
            BlueDugonQuestStatus.None => BlueDugonQuestStatus.Started,
            BlueDugonQuestStatus.Started => BlueDugonQuestStatus.KilledTurtle,
            BlueDugonQuestStatus.KilledTurtle => BlueDugonQuestStatus.ReturnedToSabonim,
            BlueDugonQuestStatus.ReturnedToSabonim => BlueDugonQuestStatus.MeditationCompleted,
            BlueDugonQuestStatus.MeditationCompleted => BlueDugonQuestStatus.Completed,
            _ => currentStatus
        };
    }
}

public enum BlueDugonQuestStatus
{
    None = 0,
    Started = 1,
    KilledTurtle = 2,
    ReturnedToSabonim = 3,
    MeditationCompleted = 4,
    Completed = 5
}