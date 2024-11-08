using Chaos.Collections;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PorteForest;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.GeneralsOfDarkness;

public class EdricGeneralsQuestScript: DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public EdricGeneralsQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
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
            case GeneralsOfDarknessQuestStatus.Started:
                Subject.Reply(
                    source,
                    "So, Thibault sent you, eh? Good! These goblins swarm faster than we can fell them.",
                    "edric_generals_first"
                );
                break;
            case GeneralsOfDarknessQuestStatus.SpokenToEdric:
                var hasTheArmor = source.Inventory.HasCountByTemplateKey("goblinArmor", 50);
                if (hasTheArmor)
                {
                    {
                        GeneralsOfDarknessQuestHelper.IncrementQuestStage(source);
                        source.Inventory.RemoveQuantityByTemplateKey("goblinArmor", 50);
                        Dialog.Reply(
                            source,
                            "Well done! This armor will serve as both a warning and a testament to our strength",
                            "edric_generals_scouting_1");
                    }
                }
                else
                {
                    Subject.Reply(
                        source,
                        "Bring me {=s50 Goblin Armor{=a. And be quick about it aisling, we dont have much time.",
                        "close"
                    );
                }
                break;
            case GeneralsOfDarknessQuestStatus.GatheredTheArmor:
                Subject.Reply(
                    source,
                    "Excellent work on gathering the goblin armor.",
                    "edric_generals_scouting_1"
                );
                break;
            case GeneralsOfDarknessQuestStatus.ScoutMissionAccepted:
                Subject.Reply(
                    source,
                    "Come back to me when you have finished scouting the Pravat Caves. We need to know what the Grimlocks are planning so we can decide on our next move.",
                    "close"
                );
                break;
            case GeneralsOfDarknessQuestStatus.ScoutedTheHearth:
                Subject.Reply(
                    source,
                    "Ah, you've returned from your scouting mission?",
                    "edric_generals_scouting_3"
                );
                break;
            case GeneralsOfDarknessQuestStatus.ReturnedToEdric:
                Subject.Reply(
                    source,
                    "You must return to Thibault at once. He must know what the Grimlocks are planning...",
                    "close"
                );
                break;

        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(source);
        if (questStatus == GeneralsOfDarknessQuestStatus.Started && optionIndex is 1)
        {
            source.Trackers.Enums.Set(GeneralsOfDarknessQuestStatus.SpokenToEdric);
        }
    }
}
