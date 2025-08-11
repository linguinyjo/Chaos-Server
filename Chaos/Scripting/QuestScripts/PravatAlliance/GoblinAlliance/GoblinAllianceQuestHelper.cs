using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;

public static class GoblinAllianceQuestHelper
{
    public static GoblinAllianceQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<GoblinAllianceQuestStatus>(out var status) ? status : GoblinAllianceQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player) =>
        player.UserStatSheet.Level is >= 21 and < 50 && GetQuestStatus(player) == GoblinAllianceQuestStatus.None;

    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(GoblinAllianceQuestStatus.Started);
    }

    public static void PlayQuestSound(Aisling player)
    {
        player.Client.SendSound(30, false);
    }
    
    public static void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(GoblinAllianceQuestStatus.Completed);
        var grimlockQuestStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        if (grimlockQuestStatus != GrimlockAllianceQuestStatus.None)
        {
            source.Trackers.Enums.Set(GrimlockAllianceQuestStatus.None);
        }

        var legendMark = new LegendMark(
            "Formed an alliance with the goblins",
            "goblinAlliance", 
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
        source.GiveExperience(25000);
        source.SendMajorQuestCompletedAnimation();
    }
}

public enum GoblinAllianceQuestStatus
{
    None = 0,
    Started = 1, 
    Completed = 2
}
