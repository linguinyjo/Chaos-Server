using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance;

/// <summary>
/// Base abstract class for alliance-based kill scripts with shared logic
/// </summary>
public abstract class AllianceKillScriptBase : MonsterScriptBase
{
    protected readonly IItemFactory ItemFactory;
    protected Aisling? Player;

    protected AllianceKillScriptBase(Monster subject, IItemFactory itemFactory)
        : base(subject)
    {
        ItemFactory = itemFactory;
    }

    /// <summary>
    /// Track the player who first attacked the monster
    /// </summary>
    public override void OnAttacked(Creature source, int damage, int? aggroOverride)
    {
        Player ??= source as Aisling;
    }

    /// <summary>
    /// Handle gemstone distribution on monster death
    /// </summary>
    public override void OnDeath()
    {
        if (Player == null) return;

        if (Player.Group == null)
        {
            TryGivePlayerGemstone(Player);
            return;
        }

        var requiredMapId = Player.Trackers.LastMapInstanceId;
        foreach (var aisling in Player.Group)
        {
            if (aisling.Trackers.LastMapInstanceId != requiredMapId) continue;
            TryGivePlayerGemstone(aisling);
        }
    }

    /// <summary>
    /// Abstract method to be implemented by specific alliance scripts
    /// Determines quest-specific logic for gemstone distribution
    /// </summary>
    protected abstract bool ShouldGiveGemstone(Aisling aisling);

    /// <summary>
    /// Abstract method to play quest-specific sound
    /// </summary>
    protected abstract void PlayQuestSound(Aisling aisling);

    /// <summary>
    /// Common logic for trying to give a gemstone to a player
    /// </summary>
    private void TryGivePlayerGemstone(Aisling aisling)
    {
        // Check if the player should receive the gemstone based on quest-specific logic
        if (!ShouldGiveGemstone(aisling)) return;

        // 10% chance of giving the item to the player 
        var random = new Random();
        if (random.Next(0, 100) >= 10) return;

        // Prevent exceeding 10 gemstones
        if (aisling.Inventory.HasCountByTemplateKey("redGemstone", 10)) return;

        // Create and add the gemstone
        var item = ItemFactory.Create("redGemstone");
        aisling.Inventory.TryAddToNextSlot(item);

        // Play quest-specific sound
        PlayQuestSound(aisling);
    }
}
