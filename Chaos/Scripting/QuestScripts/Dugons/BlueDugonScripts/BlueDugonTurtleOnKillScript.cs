using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class BlueDugonTurtleOnKillScript : MonsterScriptBase
{
    private Aisling? player;
    private readonly BlueDugonQuestHelper blueDugonQuestHelper = new();

    /// <inheritdoc />
    public BlueDugonTurtleOnKillScript(Monster subject)
        : base(subject) {}

    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage, int? aggroOverride)
    {
        player ??= source as Aisling; 
    }
    
    public override void OnDeath()
    {
        if (player == null) return;
        var questStatus = blueDugonQuestHelper.GetQuestStatus(player);
        if (questStatus != BlueDugonQuestStatus.Started) return;
        blueDugonQuestHelper.HandleKill(player);
        player.SendOrangeBarMessage("You feel the presence of your sabonim watching over you");
        player.Client.SendSound(47, false);
        player.Client.SendAnimation(
            new Animation { AnimationSpeed = 150, TargetAnimation = 22 }
        );
    }
}
