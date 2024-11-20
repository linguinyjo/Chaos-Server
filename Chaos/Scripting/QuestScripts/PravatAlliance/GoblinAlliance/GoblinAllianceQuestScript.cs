using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;

public class GoblinAllianceQuestScript: DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IEffectFactory EffectFactory;
    
    /// <inheritdoc />
    public GoblinAllianceQuestScript(Dialog subject, IEffectFactory effectFactory)
        : base(subject)
    {
        Dialog = subject;
        EffectFactory = effectFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = GoblinAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GoblinAllianceQuestStatus.Started:
                var hasGemstones = source.Inventory.HasCountByTemplateKey("redGemstone", 10);
                if (hasGemstones)
                {
                    GoblinAllianceQuestHelper.CompleteQuest(source);
                    source.Inventory.RemoveQuantityByTemplateKey("redgemstone", 10);
                    Dialog.Reply(
                        source,
                        "Yes... you return with my precious gemstones! You've proven yourself capable. The Goblins acknowledge your strength.",
                        "goblin_alliance_completed");
                }

                break;
            case GoblinAllianceQuestStatus.Completed:
                var effect = EffectFactory.Create("goblinsBlessingBuff");
                source.Effects.Apply(source, effect);
                break;
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = GoblinAllianceQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GoblinAllianceQuestStatus.None:
                if (optionIndex is 1)
                {
                    GoblinAllianceQuestHelper.StartQuest(source);
                }
                break;
        }
    }
}
