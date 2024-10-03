using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts;
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
        if (player?.Group == null) return;
        foreach (var aisling in player.Group)
        {
            TerrorQuestHelper.IncrementQuestStage(aisling);
            aisling.TraverseMap(targetMap, Destination);
        }
    } 
    
    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage)
    {
        player = source as Aisling;
    } 
}
