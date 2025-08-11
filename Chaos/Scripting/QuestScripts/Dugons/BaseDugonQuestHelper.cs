using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Dugons;

    
public abstract class BaseQuestHelper<TStatus> where TStatus : Enum
{
    protected abstract string QuestId { get; }
    protected abstract Dugon Dugon { get; }
    protected abstract string QuestBlockId { get; }
    protected abstract string QuestName { get; }
    protected abstract int RequiredLevel { get; }
    protected abstract TStatus CompletedStatus { get; }
    protected abstract TStatus NoneStatus { get; }
    protected abstract TimeSpan AllowedTime { get; }
    
    public TStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<TStatus>(out var status) ? status : NoneStatus;
    }

    public bool IsEligible(Aisling player)
    {
        var status = GetQuestStatus(player);
        return player.StatSheet.Level >= RequiredLevel
            && player.HasClass(BaseClass.Monk)
            && !Equals(status, CompletedStatus) 
            && Equals(status, NoneStatus);
    }

    public bool StartQuest(Aisling player)
    {
        // if (!IsEligible(player)) return false;
        if (player.Trackers.TimedEvents.HasActiveEvent(QuestBlockId, out _)) return false;
        player.Trackers.Enums.Set(GetNextStatus(NoneStatus));
        player.Trackers.TimedEvents.AddEvent(QuestId, AllowedTime);
        return true;
    }

    public void HandleKill(Aisling player)
    {
        var status = GetQuestStatus(player);
        player.Trackers.Enums.Set(GetNextStatus(status));
        player.Trackers.TimedEvents.TryConsumeEvent(QuestId, out _);
    }

    public bool HasPlayerFailed(Aisling player)
    {
        var playerFailed = player.Trackers.TimedEvents.TryConsumeEvent(QuestId, out _);
        if (!playerFailed) return false;
        player.Trackers.Enums.Set(NoneStatus);
        player.Trackers.TimedEvents.AddEvent(QuestBlockId, TimeSpan.FromSeconds(86400), true);
        return true;
    }

    public bool IsPlayerBlocked(Aisling player)
    {
        return player.Trackers.TimedEvents.HasActiveEvent(QuestBlockId, out _);
    }

    public void HandleMeditationFailed(Aisling player)
    {
        var status = GetQuestStatus(player);
        player.Trackers.Enums.Set(GetNextStatus(status));
        player.Trackers.TimedEvents.AddEvent(QuestBlockId, TimeSpan.FromSeconds(86400), true);
    }

    public void HandleMeditationSuccess(Aisling player)
    {
        var status = GetQuestStatus(player);
        player.Trackers.Enums.Set(GetNextStatus(status));
    }

    public void IncrementQuestStage(Aisling player)
    {
        var status = GetQuestStatus(player);
        if (Equals(status, CompletedStatus)) return;
        
        player.Trackers.Enums.Set(GetNextStatus(status));
    }

    public void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(CompletedStatus);
        var legendMark = new LegendMark(
            $"Earned the {QuestName} Dugon",
            "dugon", 
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddOrReplace(legendMark);
        source.Trackers.Enums.Set(Dugon);
        source.SendMajorQuestCompletedAnimation();
    }

    protected abstract TStatus GetNextStatus(TStatus currentStatus);
}