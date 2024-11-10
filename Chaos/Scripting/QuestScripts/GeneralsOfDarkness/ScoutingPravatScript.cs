using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class ScoutingPravatScript : ReactorTileScriptBase
{

    /// <inheritdoc />
    public ScoutingPravatScript(ReactorTile subject)
        : base(subject)
    {}

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(aisling);
        if (questStatus != GeneralsOfDarknessQuestStatus.ScoutMissionAccepted) return;
        
        aisling.SendOrangeBarMessage("Successfully scouted the Pravat Caves");
        GeneralsOfDarknessQuestHelper.IncrementQuestStage(aisling);
        GeneralsOfDarknessQuestHelper.PlayQuestSound(aisling);
    }
}