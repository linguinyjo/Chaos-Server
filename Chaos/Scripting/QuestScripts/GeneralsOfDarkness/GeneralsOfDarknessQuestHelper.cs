using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public static class GeneralsOfDarknessQuestHelper
{
    public static GeneralsOfDarknessQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<GeneralsOfDarknessQuestStatus>(out var status) ? status : GeneralsOfDarknessQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player) =>
        player.UserStatSheet.Level is >= 21 and < 50 && GetQuestStatus(player) == GeneralsOfDarknessQuestStatus.None;
    

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(GeneralsOfDarknessQuestStatus.Started);
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == GeneralsOfDarknessQuestStatus.Completed) return;
        
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void PlayQuestSound(Aisling player)
    {
        player.Client.SendSound(29, false);
    }
    
    public static void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(GeneralsOfDarknessQuestStatus.Completed);
        var legendMark = new LegendMark(
            "Champion of Loures, slayed the generals of darkness",
            "GeneralsOfDarkness", 
            MarkIcon.Victory,
            MarkColor.Blue,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
        source.GiveExperience(300000);
        source.SendQuestCompletedAnimation();
    }
}

public enum GeneralsOfDarknessQuestStatus
{
    None = 0,
    Started = 1, 
    SpokenToEdric = 2,
    GatheredTheArmor = 3,
    ScoutMissionAccepted = 4,
    ScoutedTheHearth = 5,
    ReturnedToEdric = 6,
    SlayTheGenerals = 7,
    Completed = 8 
}
