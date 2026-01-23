using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class GreenDugonQuestHelper : BaseQuestHelper<GreenDugonQuestStatus>
{
    protected override string QuestId => "GreenDugon";
    protected override Dugon Dugon => Dugon.Green;
    protected override string QuestBlockId => "GreenDugonBlock";
    protected override string QuestName => "Green";
    protected override int RequiredLevel => 22;
    protected override GreenDugonQuestStatus CompletedStatus => GreenDugonQuestStatus.Completed;
    protected override GreenDugonQuestStatus NoneStatus => GreenDugonQuestStatus.None;
    protected override TimeSpan AllowedTime => TimeSpan.FromMinutes(30);

    protected override GreenDugonQuestStatus GetNextStatus(GreenDugonQuestStatus currentStatus)
    {
        return currentStatus switch
        {
            GreenDugonQuestStatus.None => GreenDugonQuestStatus.Started,
            GreenDugonQuestStatus.Started => GreenDugonQuestStatus.KilledCrab,
            GreenDugonQuestStatus.KilledCrab => GreenDugonQuestStatus.ReturnedToSabonim,
            GreenDugonQuestStatus.ReturnedToSabonim => GreenDugonQuestStatus.MeditationCompleted,
            GreenDugonQuestStatus.MeditationCompleted => GreenDugonQuestStatus.Completed,
            _ => currentStatus
        };
    }
}

public enum GreenDugonQuestStatus
{
    None = 0,
    Started = 1,
    KilledCrab = 2,
    ReturnedToSabonim = 3,
    MeditationCompleted = 4,
    Completed = 5
}