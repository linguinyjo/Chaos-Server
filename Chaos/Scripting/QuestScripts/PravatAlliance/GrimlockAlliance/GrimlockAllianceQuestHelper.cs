using Chaos.DarkAges.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.World;
using Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;

public static class GrimlockAllianceQuestHelper
{
    public static GrimlockAllianceQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<GrimlockAllianceQuestStatus>(out var status)
            ? status
            : GrimlockAllianceQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player) =>
        player.UserStatSheet.Level is >= 21 and < 41 && GetQuestStatus(player) == GrimlockAllianceQuestStatus.None;


    public static void StartQuest(Aisling player)
    {
        player.Trackers.Enums.Set(GrimlockAllianceQuestStatus.Started);
    }

    public static void PlayQuestSound(Aisling player)
    {
        player.Client.SendSound(30, false);
    }

    public static void CompleteQuest(Aisling source)
    {
        source.Trackers.Enums.Set(GrimlockAllianceQuestStatus.Completed);
        var goblinQuestStatus = GoblinAllianceQuestHelper.GetQuestStatus(source);
        if (goblinQuestStatus != GoblinAllianceQuestStatus.None)
        {
            source.Trackers.Enums.Set(GoblinAllianceQuestStatus.None);
        }

        var legendMark = new LegendMark(
            "Formed an alliance with the grimlocks",
            "grimlockAlliance",
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
        source.GiveExperience(25000);
        source.SendMajorQuestCompletedAnimation();
    }
}

public enum GrimlockAllianceQuestStatus
{
    None = 0,
    Started = 1,
    Completed = 2
}