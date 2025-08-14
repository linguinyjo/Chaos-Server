using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class FaerieReactorTileScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public FaerieReactorTileScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(aisling);
        if (questStatus != FromTheHeartQuestStatus.SpokenToJean) return;
        var randomItem = ItemFactory.Create("redScarf");
        var template = DialogFactory.Create("red_scarf", randomItem);
        template.Display(aisling);
    }
}