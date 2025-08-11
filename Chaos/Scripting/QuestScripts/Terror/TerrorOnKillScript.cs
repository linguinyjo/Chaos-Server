using Chaos.Collections;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorOnKillScript : ConfigurableMonsterScriptBase
{
    private readonly ISimpleCache SimpleCache;
    private Aisling? player;

    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    #endregion
    
    /// <inheritdoc />
    public TerrorOnKillScript(Monster subject, ISimpleCache simpleCache)
        : base(subject)
    {
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDeath()
    {
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        if (player == null) return;
        if (player?.Group == null)
        {
            TerrorQuestHelper.IncrementQuestStage(player);
            player.TraverseMap(targetMap, Destination);
            return;
        }
        var requiredMapId = player.Trackers.LastMapInstanceId;
        foreach (var aisling in player.Group)
        {
            if (aisling.Trackers.LastMapInstanceId != requiredMapId) continue;
            TerrorQuestHelper.IncrementQuestStage(aisling);
            aisling.TraverseMap(targetMap, Destination);
        }
    } 
    
    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage)
    {
        player ??= source as Aisling;
    } 
}
