using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class PorteForestTrapScript : ReactorTileScriptBase
{
    private readonly Animation Animation = new()
    {
        AnimationSpeed = 150,
        TargetAnimation = 50
    };

    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public PorteForestTrapScript(ReactorTile subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;

        var hasWing = aisling.Inventory.HasCountByTemplateKey("flyingAntWing", 1);
        if (hasWing)
        {
            aisling.Client.SendSound(30, false);
            aisling.Inventory.RemoveQuantityByTemplateKey("flyingAntWing", 1);
        }
        else
        {
            aisling.StatSheet.SubtractHp(aisling.StatSheet.CurrentHp);
            aisling.Client.SendAttributes(StatUpdateType.Vitality);
            if (!aisling.IsAlive) aisling.Script.OnDeath();
        }
    }
}