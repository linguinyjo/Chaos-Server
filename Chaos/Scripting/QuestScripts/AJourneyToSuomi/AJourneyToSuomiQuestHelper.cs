using Chaos.Models.Data;
using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts.AJourneyToSuomi;

public static class AJourneyToSuomiQuestHelper
{
    public static AJourneyToSuomiQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<AJourneyToSuomiQuestStatus>(out var status)
            ? status
            : AJourneyToSuomiQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        var status = GetQuestStatus(player);
        return player.StatSheet.Level > 3 && status != AJourneyToSuomiQuestStatus.Completed;
    }

    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == AJourneyToSuomiQuestStatus.Completed) return;
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(AJourneyToSuomiQuestStatus.FetchFruitShipment);
    }

    public static void CompleteQuest(Aisling player)
    {
        player.Trackers.Enums.Set(AJourneyToSuomiQuestStatus.Completed);
        player.TryGiveGold(5000);
        player.GiveExperience(500);
        player.Client.SendAnimation(new Animation()
        {
            AnimationSpeed = 150,
            TargetAnimation = 22
        });
        player.Client.SendSound(29, false);
    }
}

public enum AJourneyToSuomiQuestStatus
{
    None = 0,
    FetchFruitShipment = 1,
    ReceivedFruitShipment = 2,
    Completed = 3
}