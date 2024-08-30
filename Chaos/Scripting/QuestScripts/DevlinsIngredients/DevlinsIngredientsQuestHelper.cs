using Chaos.Common.Definitions;
using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts.DevlinsIngredients;

public static class DevlinsIngredientsQuestHelper
{
    public static DevlinsIngredientsQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<DevlinsIngredientsQuestStatus>(out var status) ? status : DevlinsIngredientsQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return player.HasClass(BaseClass.Peasant) && GetQuestStatus(player) != DevlinsIngredientsQuestStatus.Completed;
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        Console.WriteLine(questStatus);
        // If the quest is already completed, no further increment
        if (questStatus == DevlinsIngredientsQuestStatus.Completed)
            return;
        
        // Increment to the next stage in the enum
        DevlinsIngredientsQuestStatus nextStatus = questStatus + 1;

        // Update the player's quest status
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(DevlinsIngredientsQuestStatus.FetchRawWax);
    }
}

public enum DevlinsIngredientsQuestStatus
{
    None = 0,
    FetchRawWax = 1,
    FetchRawHoney = 2,
    OneFinalTask = 3,
    ToShinewood = 4,
    Completed = 8
}