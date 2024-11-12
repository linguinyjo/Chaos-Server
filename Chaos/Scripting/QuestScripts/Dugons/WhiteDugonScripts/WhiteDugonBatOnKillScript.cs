using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class WhiteDugonBatOnKillScript : MonsterScriptBase
{
    private Aisling? player;
    
    /// <inheritdoc />
    public WhiteDugonBatOnKillScript(Monster subject)
        : base(subject) {}

    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage, int? aggroOverride)
    {
        player ??= source as Aisling; 
    }
    
    public override void OnDeath()
    {
        if (player == null) return;
        var questStatus = WhiteDugonQuestHelper.GetQuestStatus(player);
        if (questStatus != WhiteDugonQuestStatus.Started) return;
        WhiteDugonQuestHelper.HandleBatKill(player);
        player.SendOrangeBarMessage("You feel the presence of your sabonim confirming your kill");
        player.Client.SendSound(47, false);
        player.Client.SendAnimation(
            new Animation { AnimationSpeed = 150, TargetAnimation = 22 }
        );
    }
}
