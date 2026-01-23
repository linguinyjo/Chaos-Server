using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class GoblinArmorDropScript : MonsterScriptBase
{
    private readonly IItemFactory ItemFactory;
    private Aisling? player;

    /// <inheritdoc />
    public GoblinArmorDropScript(Monster subject, IItemFactory itemFactory)
        : base(subject)
    {
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage, int? aggroOverride)
    {
        player ??= source as Aisling;
    }

    public override void OnDeath()
    {
        if (player == null) return;
        if (player?.Group == null)
        {
            GivePlayerGoblinArmor(player);
            return;
        }

        var requiredMapId = player?.GetCurrentLocation().Map;
        foreach (var aisling in player.Group)
        {
            if (player?.GetCurrentLocation().Map != requiredMapId) continue;
            GivePlayerGoblinArmor(aisling);
        }
    }

    private void GivePlayerGoblinArmor(Aisling aisling)
    {
        if (GeneralsOfDarknessQuestHelper.GetQuestStatus(aisling) !=
            GeneralsOfDarknessQuestStatus.SpokenToEdric) return;
        if (aisling.Inventory.HasCountByTemplateKey("goblinArmor", 50)) return;
        var item = ItemFactory.Create("goblinArmor");
        aisling.Inventory.TryAddToNextSlot(item);
    }
}