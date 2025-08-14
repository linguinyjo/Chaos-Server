using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public static class FromTheHeartQuestHelper
{
    
    public static FromTheHeartQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<FromTheHeartQuestStatus>(out var status) ? status : FromTheHeartQuestStatus.None;
    }
    
    public static bool IsQuestAvailable(Aisling player)
    {
        var status = GetQuestStatus(player);
        return player.StatSheet.Level > 11 && status == FromTheHeartQuestStatus.None;
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(FromTheHeartQuestStatus.SpokenToMarlin);
    }

    public static void CompleteQuest(Aisling source)
    {
        
    }
}

public enum FromTheHeartQuestStatus
{
    None = 0,
    SpokenToMarlin = 1,
    SpokenToJean = 2,
    SpokenToFaerie = 3,
    SpokenToMarlinAgain = 4,
    FetchScorpionSting = 5,
    Completed = 6,
}