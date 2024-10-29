using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.SpareAStick;

public class CalloughSpareAStickScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    
    /// <inheritdoc />
    public CalloughSpareAStickScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = SpareAStickQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case SpareAStickQuestStatus.None:
                break;
            case SpareAStickQuestStatus.Started:
            {
                var hasTheBranches = source.Inventory.HasCountByTemplateKey("treeBranch", 6);
                if (hasTheBranches)
                {   
                    Dialog.Reply(
                        source,
                        "Ah, these look like they'll do the job. Take this stick and wooden shield. Oh, and come back to me when you've made yourself some gold... then you can buy yourself a real weapon!",
                        "Close"
                    );
                    source.Inventory.RemoveQuantityByTemplateKey("treeBranch", 6);
                    var stick = ItemFactory.Create("stick");
                    var woodenShield = ItemFactory.Create("woodenShield");
                    source.Inventory.TryAddToNextSlot(stick);
                    source.Inventory.TryAddToNextSlot(woodenShield);
                    SpareAStickQuestHelper.CompleteQuest(source);
                }
                else
                {
                    Dialog.Reply(
                        source,
                        "I'm still waiting for those branches. Be quick about it now, I don't have all day ",
                        "Close"
                    );
                }
                break;
            }
            case SpareAStickQuestStatus.Completed:
                return;
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var questStatus = SpareAStickQuestHelper.GetQuestStatus(source);
        if (questStatus is not SpareAStickQuestStatus.None || optionIndex != 1) return;
        SpareAStickQuestHelper.StartQuest(source);
        Dialog.Reply(
            source,
            "Great. Just wander around some of the trees nearby and pick up any solid looking branches you can find.",
            "Close"
        );
    }
}
