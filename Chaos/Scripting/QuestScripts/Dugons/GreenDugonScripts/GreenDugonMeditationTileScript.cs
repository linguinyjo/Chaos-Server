using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class GreenDugonMeditationTileScript : ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper GreenDugonQuestHelper = new();

    /// <inheritdoc />
    public GreenDugonMeditationTileScript(ReactorTile subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        if (GreenDugonQuestHelper.GetQuestStatus(aisling) != GreenDugonQuestStatus.ReturnedToSabonim) return;
        if (GreenDugonQuestHelper.IsPlayerBlocked(aisling)) return;

        var newDialog = new Dialog(
            aisling,
            DialogFactory,
            ChaosDialogType.Normal,
            "*A profound sense of peace washes over you. The gentle flow of the sacred waters seems to whisper ancient wisdom...*")
        {
            NextDialogKey = "green_dugon_meditation_1"
        };
        newDialog.Display(aisling);
    }
}