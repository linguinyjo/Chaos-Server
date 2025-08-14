using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;

public class GrimlockAllianceQuestScript: DialogScriptBase
{
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public GrimlockAllianceQuestScript(Dialog subject)
        : base(subject)
    {
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (source.StatSheet.Level is >= 41 or < 21) return;
        var questStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        if (questStatus != GrimlockAllianceQuestStatus.Started) return;
        Subject.AddOption("I have some here", "grimlock_i_have_some_here");
        if (Subject.Template.TemplateKey != "grimlock_i_have_some_here") return;
        
        var count = source.Inventory.CountOfByTemplateKey("conixStone");
        if (count <= 0)
        {
            Dialog.Reply(
                source, 
                "You return to me empty handed?", 
                "close");
            return;
        }
        var expReward = count * 5000;
        source.Inventory.RemoveQuantityByTemplateKey("conixStone", count);
        source.GiveExperience(expReward);
        Dialog.Reply(
            source, 
            "Ah yes, these will do just fine. I can feel them raidating with power.", 
            "grimlock_alliance_completed");
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        if (optionIndex is not 1 || !GrimlockAllianceQuestHelper.IsQuestAvailable(source)) return;
        if (questStatus == GrimlockAllianceQuestStatus.None) GrimlockAllianceQuestHelper.StartQuest(source);
    }
}
