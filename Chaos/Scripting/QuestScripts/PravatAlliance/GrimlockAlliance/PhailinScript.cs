using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;

public class PhailinScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public PhailinScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (GoblinAllianceQuestHelper.GetQuestStatus(source) == GoblinAllianceQuestStatus.Completed)
        {
            Subject.Reply(source, "Filthy, traitorous scum! Leave here before my guards slay you where you stand.");
        }
        if (source.StatSheet.Level >= 50) return;
        var questStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GrimlockAllianceQuestStatus.None:
                Subject.AddOption("Grimlock alliance", "grimlock_alliance_1");
                break;
            case GrimlockAllianceQuestStatus.Started:
                Subject.AddOption("Grimlock alliance", "grimlock_alliance_gemstones_1");
                break;
            case GrimlockAllianceQuestStatus.Completed:
                Subject.AddOption("Grimlock alliance", "grimlock_alliance_buff");
                break;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
