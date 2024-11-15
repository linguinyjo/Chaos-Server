using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.SpareAStick;

public class TreeBranchScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public TreeBranchScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        
        var questStatus = SpareAStickQuestHelper.GetQuestStatus(aisling);
        if (questStatus != SpareAStickQuestStatus.Started) return;
        
        // Generate random number between 0 and 100
        var random = new Random();
        var chance = random.Next(1, 101);
    
        // 5% chance (if number is 1-5)
        if (chance > 15) return;
        var item = ItemFactory.Create("treeBranch");
        aisling.Inventory.TryAddToNextSlot(item);
    }
}