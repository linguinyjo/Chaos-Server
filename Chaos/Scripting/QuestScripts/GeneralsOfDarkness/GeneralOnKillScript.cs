using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class GeneralOnKillScript : ConfigurableMonsterScriptBase
{
    private readonly IItemFactory ItemFactory;
    private Aisling? player;

    /// <inheritdoc />
    public GeneralOnKillScript(Monster subject, IItemFactory itemFactory)
        : base(subject)
    {
        ItemFactory = itemFactory;
    }

    #region ScriptVars

    protected string? ItemTemplateKey { get; init; }

    #endregion

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
            GivePlayerGoblinHeartArmor(player);
            return;
        }

        var requiredMapId = player?.GetCurrentLocation().Map;
        foreach (var aisling in player.Group)
        {
            if (aisling.GetCurrentLocation().Map != requiredMapId) continue;
            GivePlayerGoblinHeartArmor(aisling);
        }
    }

    private void GivePlayerGoblinHeartArmor(Aisling aisling)
    {
        if (GeneralsOfDarknessQuestHelper.GetQuestStatus(aisling) !=
            GeneralsOfDarknessQuestStatus.SlayTheGenerals) return;
        if (aisling.Inventory.HasCountByTemplateKey(ItemTemplateKey, 1)) return;
        var item = ItemFactory.Create(ItemTemplateKey);
        aisling.Inventory.TryAddToNextSlot(item);
    }
}