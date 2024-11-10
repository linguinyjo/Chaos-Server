using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Dugons;

public static class DugonQuestHelper
{
    public static DugonQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<DugonQuestStatus>(out var status) ? status : DugonQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return player.HasClass(BaseClass.Monk) && GetQuestStatus(player) != DugonQuestStatus.Completed;
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        
    }

    public static void StartQuest(Aisling player)
    {
        // Player must be a monk, and be level 11
        player.Trackers.Enums.Set(DugonQuestStatus.WhiteStarted);
    }

    public static void CompleteQuest(Aisling source)
    {
       
    }
}

public enum DugonQuestStatus
{
    None = 0,
    WhiteStarted = 1,
    WhiteCompleted = 2,
    GreenStarted = 3,
    GreenCompleted = 4,
    Completed = 5
}