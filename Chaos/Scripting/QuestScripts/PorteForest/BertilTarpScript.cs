using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class BertilTarpScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    
    /// <inheritdoc />
    public BertilTarpScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (!PorteForestQuestHelper.IsElligibleToMakeTarp(source)) return;
        // does the player already have a tarp?
        if (source.Inventory.ContainsByTemplateKey("silverFurTarp") || source.Equipment.ContainsByTemplateKey("silverFurTarp"))
        {
            Subject.Reply(source, "I've already given you a tarp.", "close");
            return;
        };
        
        var hasFur = source.Inventory.HasCountByTemplateKey("silverFur", 2);
        if (!hasFur || source.Gold < 10000) {
            Subject.Reply(source, "What do you take me for, a fool? Come back when you have what I asked you for.", "close");
            return;
        };
            
        var item = ItemFactory.Create("silverFurTarp");
        var tarpGiven = source.Inventory.TryAddToNextSlot(item);
        if (!tarpGiven) return;
        source.Inventory.RemoveQuantityByTemplateKey("silverFur", 2);
        source.TryTakeGold(10000);
        Subject.Reply(source, "Here's the tarp as promised. Good luck if you're planning on venturing deep into Porte Forest. There's not many aislings brave enough to do that anymore...", "close");
    }
    
    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
