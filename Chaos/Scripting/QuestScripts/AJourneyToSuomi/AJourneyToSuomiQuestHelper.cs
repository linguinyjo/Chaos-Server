using System.Diagnostics.Eventing.Reader;
using Chaos.Common.Definitions;
using Chaos.MetaData.EventMetaData;
using Chaos.Models.Data;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.AJourneyToSuomi;

            // public class AJourneyToSuomiQuestHelper : QuestHelper<AJourneyToSuomiQuestStatus>
            // {
            //     public override void StartQuest(Aisling player)
            //     {
            //         player.Trackers.Enums.Set(AJourneyToSuomiQuestStatus.FetchFruitShipment);
            //     }
            //
            //     public override void CompleteQuest(Aisling player)
            //     {
            //         player.Trackers.Enums.Set(AJourneyToSuomiQuestStatus.Completed);
            //         var legendMark = new LegendMark(
            //             "Delivered Oona's shipment of fruit",
            //             "aJourneyToSuomi",
            //             MarkIcon.Victory,
            //             MarkColor.White,
            //             1,
            //             GameTime.Now);
            //         player.Legend.AddUnique(legendMark);
            //     }
            //
            //     protected override AJourneyToSuomiQuestStatus GetCompletedStatus() => AJourneyToSuomiQuestStatus.Completed;
            //     protected override AJourneyToSuomiQuestStatus GetInitialStatus() => AJourneyToSuomiQuestStatus.FetchFruitShipment;
            // }

public static class AJourneyToSuomiQuestHelper
{
    public static AJourneyToSuomiQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<AJourneyToSuomiQuestStatus>(out var status) ? status : AJourneyToSuomiQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        var status = GetQuestStatus(player);
        return player.StatSheet.Level < 41 && status != AJourneyToSuomiQuestStatus.Completed;
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
        var legendMark = new LegendMark(
            "Delivered Oona's shipment of fruit",
            "aJourneyToSuomi",
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        player.TryGiveGold(2500);
        player.Legend.AddUnique(legendMark);
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