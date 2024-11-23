using Chaos.Collections;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.DialogScripts.Tagor;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.ReactorTileScripts;

public class WarriorTrainingGroundWarpScript : ReactorTileScriptBase
{
    private readonly ISimpleCache SimpleCache;
    private readonly IWarriorMembershipHelper WarriorMembershipHelper;

    /// <inheritdoc />
    public WarriorTrainingGroundWarpScript(
        ReactorTile subject, 
        ISimpleCache simpleCache, 
        IWarriorMembershipHelper? warriorMembershipHelper = null
    ) : base(subject)
    {
        SimpleCache = simpleCache;
        WarriorMembershipHelper = warriorMembershipHelper ?? new WarriorMembershipHelper();
    }
    /// <inheritdoc />
    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        if(WarriorMembershipHelper.MembershipIsActive(aisling) == null) return;
        var targetMap = SimpleCache.Get<MapInstance>("warriorHallway");
        var destination = new Location("warriorHallway",3, 14);
        source.TraverseMap(targetMap, destination);
        
    }
}
