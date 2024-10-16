using Chaos.Collections;
using Chaos.Extensions.Geometry;
using Chaos.Geometry.Abstractions;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorQuestWarpScript : ConfigurableReactorTileScriptBase
{
    private readonly ISimpleCache SimpleCache;

    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    protected TerrorQuestStatus RequiredStatus { get; init; } = TerrorQuestStatus.None;
    #endregion

    /// <inheritdoc />
    public TerrorQuestWarpScript(ReactorTile subject, ISimpleCache simpleCache)
        : base(subject)
        => SimpleCache = simpleCache;

    /// <inheritdoc />
    public override void OnWalkedOn(Creature source)
    {
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        if (source is Aisling aisling && TerrorQuestHelper.GetQuestStatus(aisling) == RequiredStatus)
        {
            source.TraverseMap(targetMap, Destination);
        }
    }
}