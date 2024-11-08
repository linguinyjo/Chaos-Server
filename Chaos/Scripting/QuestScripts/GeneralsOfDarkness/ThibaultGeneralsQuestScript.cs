using Chaos.Collections;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PorteForest;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class ThibaultGeneralsQuestScript: DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public ThibaultGeneralsQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GeneralsOfDarknessQuestStatus.ReturnedToEdric:
                Subject.Reply(
                    source,
                    "So its true. The grimlocks and goblins are more organized than we feared. This madness will not end until we cut off the head of the serpent. The leaders of these wretched hordes must be slain.",
                    "thibault_slay_them_1"
                );
                break;
            case GeneralsOfDarknessQuestStatus.SlayTheGenerals:
            {
                var hasSlainTheChief = source.Inventory.HasCountByTemplateKey("goblinChiefHeart", 1);
                var hasSlainTheQueen = source.Inventory.HasCountByTemplateKey("grimlockQueenHeart", 1);

                if (hasSlainTheChief && hasSlainTheQueen)
                {
                    source.Inventory.RemoveQuantityByTemplateKey("goblinChiefHeart", 1);
                    source.Inventory.RemoveQuantityByTemplateKey("grimlockQueenHeart", 1);
                    GeneralsOfDarknessQuestHelper.CompleteQuest(source);
                    Subject.Reply(
                        source,
                        "I can hardly believe it... the chief and the queen, both slain!",
                        "thibault_generals_completed"
                    );
                }
                else
                {
                    Subject.Reply(
                        source,
                        "The generals still breathe... and while they do, Loures is plagued with the fear of ruin. Return to me when you have the proof of their demise.",
                        "close"
                    );
                }
                break;
            }
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(source);
        if (questStatus == GeneralsOfDarknessQuestStatus.None && optionIndex is 1)
        {
            GeneralsOfDarknessQuestHelper.StartQuest(source);
        }
    }
}
