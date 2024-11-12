using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public static class WhiteDugonQuestHelper
{
    public static WhiteDugonQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<WhiteDugonQuestStatus>(out var status) ? status : WhiteDugonQuestStatus.None;
    }

    public static bool IsEligible(Aisling player)
    {
        return player.StatSheet.Level >= 11 && player.HasClass(BaseClass.Monk)
                                            && GetQuestStatus(player) != WhiteDugonQuestStatus.Completed 
                                            && GetQuestStatus(player) == WhiteDugonQuestStatus.None;
    }

    public static bool StartQuest(Aisling player)
    {
        if(player.Trackers.TimedEvents.HasActiveEvent(WhiteDugonBlock, out _)) return false;
        player.Trackers.Enums.Set(WhiteDugonQuestStatus.Started);
        player.Trackers.TimedEvents.AddEvent(WhiteDugonId, TimeSpan.FromMinutes(30));
        return true;
    }
    
    public static void HandleBatKill(Aisling player)
    {
        player.Trackers.Enums.Set(WhiteDugonQuestStatus.KilledBat);
        player.Trackers.TimedEvents.TryConsumeEvent(WhiteDugonId, out _);
    }

    public static bool HasPlayerFailed(Aisling player)
    {
        var playerFailed = player.Trackers.TimedEvents.TryConsumeEvent(WhiteDugonId, out _);
        if (!playerFailed) return false;
        player.Trackers.Enums.Set(WhiteDugonQuestStatus.None);
        player.Trackers.TimedEvents.AddEvent(WhiteDugonBlock, TimeSpan.FromSeconds(86400), true);
        return true;
    }

    public static bool IsPlayerBlocked(Aisling player)
    {
        return player.Trackers.TimedEvents.HasActiveEvent(WhiteDugonBlock, out _);
    }
    
    public static void HandleMeditationFailed(Aisling player)
    {
        player.Trackers.Enums.Set(WhiteDugonQuestStatus.ReturnedToSabonim);
        player.Trackers.TimedEvents.AddEvent(WhiteDugonBlock, TimeSpan.FromSeconds(86400), true);
    }
    
    public static void HandleMeditationSuccess(Aisling player)
    {
        player.Trackers.Enums.Set(WhiteDugonQuestStatus.MeditationCompleted);
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == WhiteDugonQuestStatus.Completed) return;
        
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(WhiteDugonQuestStatus.Completed);
        var legendMark = new LegendMark(
            "Earned the White Dugon",
            "dugon", 
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddOrReplace(legendMark);
        source.Trackers.Enums.Set(Dugon.White);

        source.SendQuestCompletedAnimation();
    }

    private const string WhiteDugonId = "WhiteDugon";
    private const string WhiteDugonBlock = "WhiteDugonId";
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
