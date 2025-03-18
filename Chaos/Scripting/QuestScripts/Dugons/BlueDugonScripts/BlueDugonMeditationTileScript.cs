using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class BlueDugonMeditationTileScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly BlueDugonQuestHelper BlueDugonQuestHelper = new();

    /// <inheritdoc />
    public BlueDugonMeditationTileScript(ReactorTile subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        if(BlueDugonQuestHelper.GetQuestStatus(aisling) != BlueDugonQuestStatus.ReturnedToSabonim) return;
        if (BlueDugonQuestHelper.IsPlayerBlocked(aisling)) return;
        
        var newDialog = new Dialog(
            aisling,
            DialogFactory,
            ChaosDialogType.Normal,
            "*A profound sense of peace washes over you. The gentle flow of the sacred waters seems to whisper ancient wisdom...*")
        {
            NextDialogKey = "blue_dugon_meditation_1"
        };
        newDialog.Display(aisling);
    }
}