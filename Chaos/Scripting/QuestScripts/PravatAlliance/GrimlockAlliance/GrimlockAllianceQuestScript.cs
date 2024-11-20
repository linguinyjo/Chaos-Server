using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;

public class GrimlockAllianceQuestScript: DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IEffectFactory EffectFactory;

    
    /// <inheritdoc />
    public GrimlockAllianceQuestScript(Dialog subject, IEffectFactory effectFactory)
        : base(subject)
    {
        Dialog = subject;
        EffectFactory = effectFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (source.StatSheet.Level >= 50) return;
        var questStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GrimlockAllianceQuestStatus.Started:
                var hasGemstones = source.Inventory.HasCountByTemplateKey("redGemstone", 10);
                if (hasGemstones)
                {
                    GrimlockAllianceQuestHelper.CompleteQuest(source);
                    source.Inventory.RemoveQuantityByTemplateKey("redgemstone", 10);
                    Dialog.Reply(
                        source, 
                        "Yes... you return with the gemstones... and the putrid stench of goblin blood! You've proven yourself capable, surface dweller. The Grimlocks acknowledge your strength.", 
                        "grimlock_alliance_completed");
                }
                break;
            case GrimlockAllianceQuestStatus.Completed:
                var effect = EffectFactory.Create("grimlocksBlessingBuff");
                source.Effects.Apply(source, effect);
                break;
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = GrimlockAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GrimlockAllianceQuestStatus.None:
                if (optionIndex is 1 && GrimlockAllianceQuestHelper.IsQuestAvailable(source))
                {
                    GrimlockAllianceQuestHelper.StartQuest(source);
                }
                break;
        }
    }
}
