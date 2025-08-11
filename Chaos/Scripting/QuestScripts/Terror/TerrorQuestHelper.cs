using System.Diagnostics;
using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Terror;


public static class TerrorQuestHelper
{
    private static string QuestBlockId => "terrorBlock";
    private const int QuestBlockTimer = 43200;
    
    public static TerrorQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<TerrorQuestStatus>(out var status) ? status : TerrorQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return player.StatSheet.Level is < 41 and > 10 && !IsPlayerBlocked(player);
    }

    private static bool IsPlayerBlocked(Aisling player)
    {
        return player.Trackers.TimedEvents.HasActiveEvent(QuestBlockId, out _);
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == TerrorQuestStatus.Completed) return;
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void StartQuest(Aisling player, TerrorLevel terrorLevel)
    {
        switch(terrorLevel)
        {
            case TerrorLevel.None:
                break;
            case TerrorLevel.Garden:
                player.Trackers.Enums.Set(TerrorQuestStatus.GardenStarted);
                break;
            case TerrorLevel.Alley:
                player.Trackers.Enums.Set(TerrorQuestStatus.AlleyStarted);
                break;
            case TerrorLevel.Crypt:
                player.Trackers.Enums.Set(TerrorQuestStatus.CryptStarted);
                break;
            default: return;
        }
    }

    public static void CompleteQuest(Aisling player)
    {
        var legendMark = new LegendMark(
            "Freed Teague from his terror",
            "terrorOfTheBeggar",
            MarkIcon.Victory,
            MarkColor.Blue,
            1,
            GameTime.Now);
       
        switch (GetQuestStatus(player))
        {
            case TerrorQuestStatus.GardenSlain:
                player.GiveExperience(20000);
                break;
            case TerrorQuestStatus.AlleySlain:
                player.GiveExperience(40000);
                break;
            case TerrorQuestStatus.CryptSlain:
                player.GiveExperience(75000);
                break;
            case TerrorQuestStatus.None:
            case TerrorQuestStatus.GardenStarted:
            case TerrorQuestStatus.AlleyStarted:
            case TerrorQuestStatus.CryptStarted:
            case TerrorQuestStatus.Completed:
            default: return;
        }
        player.Trackers.Enums.Set(TerrorQuestStatus.None);
        player.Legend.AddOrAccumulate(legendMark);
        player.SendMinorQuestCompletedAnimation();
        player.Trackers.TimedEvents.AddEvent(QuestBlockId, TimeSpan.FromSeconds(QuestBlockTimer), true);
    }

    public static TerrorLevel GetTerrorLevel(Aisling source)
    {
        return source.UserStatSheet.Level switch
        {
            >= 11 and <= 20 => TerrorLevel.Garden,
            >= 21 and <= 30 => TerrorLevel.Alley,
            >= 31 and <= 40 => TerrorLevel.Crypt,
            _ => TerrorLevel.None
        };
    }
}

public enum TerrorQuestStatus
{
    None = 0,
    GardenStarted = 1,
    GardenSlain = 2,
    AlleyStarted = 3,
    AlleySlain = 4,
    CryptStarted = 5,
    CryptSlain = 6,
    Completed = 9
}

public enum TerrorLevel
{
    None,
    Garden,
    Alley,
    Crypt
}