using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts.DarkThings;

public static class DarkThingsQuestHelper
{
    private const int QuestBlockTimer = 20;

    private static readonly string[] RequiredItems =
    [
        "spidersEye",
        "centipedeGland",
        "spidersSilk",
        "scorpionSting",
        "batWing"
    ];

    private static string QuestBlockId => "darkThingsBlock";

    public static DarkThingsQuestStatus GetQuestStatus(Aisling player)
    {
        return player.Trackers.Enums.TryGetValue<DarkThingsQuestStatus>(out var status)
            ? status
            : DarkThingsQuestStatus.None;
    }

    public static bool IsQuestAvailable(Aisling player)
    {
        return !player.Trackers.TimedEvents.HasActiveEvent(QuestBlockId, out _);
    }

    public static string StartQuest(Aisling player)
    {
        // Randomly select one of the 5 items
        var random = new Random();
        var randomIndex = random.Next(RequiredItems.Length);

        // Map array index to enum value (adding 1 since None = 0)
        var selectedStatus = (DarkThingsQuestStatus)(randomIndex + 1);
        player.Trackers.Enums.Set(selectedStatus);
        return GetItemDisplayName(selectedStatus);
    }

    public static void CompleteQuest(Aisling source)
    {
        source.GiveExperience(4000);
        source.Trackers.Enums.Set(DarkThingsQuestStatus.None);
        source.SendMinorQuestCompletedAnimation();
        source.Trackers.TimedEvents.AddEvent(QuestBlockId, TimeSpan.FromSeconds(QuestBlockTimer), true);
    }

    // Helper method to get the required item key for current quest status
    private static string GetRequiredItemKey(DarkThingsQuestStatus status)
    {
        return status switch
        {
            DarkThingsQuestStatus.FetchSpidersEye => "spidersEye",
            DarkThingsQuestStatus.FetchCentipedeGland => "centipedeGland",
            DarkThingsQuestStatus.FetchSpidersSilk => "spidersSilk",
            DarkThingsQuestStatus.FetchBatWing => "batWing",
            DarkThingsQuestStatus.FetchScorpionSting => "scorpionSting",
            _ => string.Empty
        };
    }

    // Helper method to get display name for the item
    public static string GetItemDisplayName(DarkThingsQuestStatus status)
    {
        return status switch
        {
            DarkThingsQuestStatus.FetchSpidersEye => "Spiders Eye",
            DarkThingsQuestStatus.FetchCentipedeGland => "Centipede Gland",
            DarkThingsQuestStatus.FetchSpidersSilk => "Spiders Silk",
            DarkThingsQuestStatus.FetchBatWing => "Bat Wing",
            DarkThingsQuestStatus.FetchScorpionSting => "Scorpion Sting",
            _ => "Unknown Item"
        };
    }

    // Check if player has the required item for their current quest
    public static bool HasRequiredItem(Aisling player, DarkThingsQuestStatus status)
    {
        var itemKey = GetRequiredItemKey(status);
        return !string.IsNullOrEmpty(itemKey) && player.Inventory.CountOfByTemplateKey(itemKey) > 0;
    }

    // Remove the required item from player's inventory
    public static void RemoveRequiredItem(Aisling player, DarkThingsQuestStatus status)
    {
        var itemKey = GetRequiredItemKey(status);
        if (!string.IsNullOrEmpty(itemKey))
        {
            player.Inventory.RemoveQuantityByTemplateKey(itemKey, 1);
        }
    }
}

public enum DarkThingsQuestStatus
{
    None = 0,
    FetchSpidersEye = 1,
    FetchCentipedeGland = 2,
    FetchSpidersSilk = 3,
    FetchBatWing = 4,
    FetchScorpionSting = 5,
}