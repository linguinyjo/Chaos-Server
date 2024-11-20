using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;

public class OzkanScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public OzkanScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (GrimlockAllianceQuestHelper.GetQuestStatus(source) == GrimlockAllianceQuestStatus.Completed)
        {
            Subject.Reply(source, "Filthy, traitorous scum! Leave here before my guards slay you where you stand.");
        }
        if (source.StatSheet.Level >= 50) return;
        var questStatus = GoblinAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GoblinAllianceQuestStatus.None:
                Subject.AddOption("Goblin alliance", "Goblin_alliance_1");
                break;
            case GoblinAllianceQuestStatus.Started:
                Subject.AddOption("Goblin alliance", "Goblin_alliance_gemstones_1");
                break;
            case GoblinAllianceQuestStatus.Completed:
                Subject.AddOption("Goblin alliance", "Goblin_alliance_buff");
                break;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
