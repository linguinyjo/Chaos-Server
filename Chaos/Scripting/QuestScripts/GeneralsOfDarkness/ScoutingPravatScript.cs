using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.QuestScripts.PorteForest;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class ScoutingPravatScript : ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public ScoutingPravatScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(aisling);
        if (questStatus != GeneralsOfDarknessQuestStatus.ScoutMissionAccepted) return;
        
        aisling.SendOrangeBarMessage("Successfully scouted the Pravat Caves.");
        GeneralsOfDarknessQuestHelper.IncrementQuestStage(aisling);
        GeneralsOfDarknessQuestHelper.PlayQuestSound(aisling);
    }
}