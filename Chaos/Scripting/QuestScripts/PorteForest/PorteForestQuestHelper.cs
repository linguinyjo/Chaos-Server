using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public static class PorteForestQuestHelper
{
    public static PorteForestQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<PorteForestQuestStatus>(out var status) ? status : PorteForestQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player) =>
        player.UserStatSheet.Level is >= 21 and < 41 && GetQuestStatus(player) != PorteForestQuestStatus.Completed;
    
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == PorteForestQuestStatus.Completed) return;
        
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(PorteForestQuestStatus.Started);
    }

    public static bool IsElligibleToMakeTarp(Aisling source)
    {
        var questStatus = GetQuestStatus(source);
        return questStatus is >= PorteForestQuestStatus.DeliveredTheRoots and < PorteForestQuestStatus.KilledTheMantis;
    }
    
    public static void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(PorteForestQuestStatus.Completed);
        var legendMark = new LegendMark(
            "Saved the daughter of Porte Forest",
            "porteForest", 
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
    }
    
    public static bool PlayerHasEasedTheSuffering(Aisling player)
    {
        player.Trackers.Enums.TryGetValue<EasedSufferingQuestStatus>(out var status);
        return status == EasedSufferingQuestStatus.Completed;
    }
    
    public static void CompleteEaseTheSuffering(Aisling source)
    {
        source.Trackers.Enums.Set(EasedSufferingQuestStatus.Completed);
        var legendMark = new LegendMark(
            "Eased the suffering of Porte Forest",
            "easedTheSuffering", 
            MarkIcon.Victory,
            MarkColor.Blue,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
        source.GiveExperience(75000);
        source.SendQuestCompletedAnimation();
    }
}

public enum PorteForestQuestStatus
{
    None = 0,
    Started = 1, 
    SpokenToTorbjorn = 2,
    DeliveredTheRoots = 3,
    FoundThePendant = 4,
    KilledTheMantis = 5,
    Completed = 6
}

public enum EasedSufferingQuestStatus
{
    Started = 0,
    Completed = 1
}