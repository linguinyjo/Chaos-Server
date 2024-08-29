using Chaos.Common.Definitions;
using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public static class TrainingQuestHelper
{
    public static TrainingQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<TrainingQuestStatus>(out var status) ? status : TrainingQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return player.HasClass(BaseClass.Peasant) && GetQuestStatus(player) != TrainingQuestStatus.Completed;
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);

        // If the quest is already completed, no further increment
        if (questStatus == TrainingQuestStatus.Completed)
            return;

        // Increment to the next stage in the enum
        TrainingQuestStatus nextStatu = questStatus + 1;

        // Update the player's quest status
        player.Trackers.Enums.Set(nextStatu);
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(TrainingQuestStatus.SpokenToRiona);
    }
}

public enum TrainingQuestStatus
{
    None = 0,
    SpokenToRiona = 1,
    SpokenToVorlof = 2,
    SpokenToDar = 3,
    CompletedDarsRequest = 4,
    FromDarToVorlof = 5,
    SpokenToTorrance = 6,
    CompletedTorrencesRequest = 7,
    Completed = 8
}