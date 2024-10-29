using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Scripting.QuestScripts.Terror;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.SpareAStick;

public static class SpareAStickQuestHelper
{
    public static SpareAStickQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<SpareAStickQuestStatus>(out var status) ? status : SpareAStickQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return player.HasClass(BaseClass.Peasant) && GetQuestStatus(player) != SpareAStickQuestStatus.Completed;
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(SpareAStickQuestStatus.Started);
    }
    
    public static void CompleteQuest(Aisling player)
    {
        player.Trackers.Enums.Set(SpareAStickQuestStatus.Completed);
        // give stick and wooden shield
        player.GiveExperience(100);
    }
}

public enum SpareAStickQuestStatus
{
    None = 0,
    Started = 1,
    Completed = 2
}