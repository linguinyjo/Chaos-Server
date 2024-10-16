using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.Terror;


public static class TerrorQuestHelper
{
    public static TerrorQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<TerrorQuestStatus>(out var status) ? status : TerrorQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        var status = GetQuestStatus(player);
        return player.StatSheet.Level < 41 && status != TerrorQuestStatus.Completed;
    }
    
    public static void IncrementQuestStage(Aisling player)
    {
        var questStatus = GetQuestStatus(player);
        if (questStatus == TerrorQuestStatus.Completed) return;
        var nextStatus = questStatus + 1;
        player.Trackers.Enums.Set(nextStatus);
    }

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(TerrorQuestStatus.GardenStarted);
    }

    public static void CompleteQuest(Aisling player)
    {
        player.Trackers.Enums.Set(TerrorQuestStatus.Completed);
        var legendMark = new LegendMark(
            "Freed Teague from his haunting nightmares",
            "terrorOfTheBeggar",
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        
        player.GiveExperience(25000);
        player.Legend.AddUnique(legendMark);
        player.Client.SendAnimation(new Animation()
        {
            AnimationSpeed = 150,
            TargetAnimation = 22
        });
        player.Client.SendSound(29, false);
    }
}

public enum TerrorQuestStatus
{
    None = 0,
    GardenStarted = 1,
    GardenSlain = 2,
    GardenCompleted = 3,
    AlleyStarted = 4,
    AlleySlain = 5,
    AlleyCompleted = 6,
    CryptStarted = 7,
    CryptSlain = 8,
    Completed = 9
}