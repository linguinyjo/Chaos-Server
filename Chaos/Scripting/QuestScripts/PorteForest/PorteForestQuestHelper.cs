using Chaos.Common.Definitions;
using Chaos.Models.World;

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

    public static void CompleteQuest(Aisling source)
    {
        // source.TryGiveGold(2000);
        // source.GiveExperience(750);
        // source.Trackers.Enums.Set(PorteForestQuestStatus.Completed);
        // var legendMark = new LegendMark(
        //     "Brought Devlin her ingredients",
        //     "devlinsIngredients", 
        //     MarkIcon.Victory,
        //     MarkColor.White,
        //     1,
        //     GameTime.Now);
        // source.Legend.AddUnique(legendMark);
        source.SendQuestCompletedAnimation();
    }
}

public enum PorteForestQuestStatus
{
    None = 0,
    Started = 1, // From this point the player can say Porte Forest to Torbjorn and Valdemar
    SpokenToTorbjorn = 2,
    DeliveredTheRoots = 3,
    FoundThePendant = 4,
    KilledTheMantis = 5,
    SavedTheDaughter = 6,
    Completed = 7
}