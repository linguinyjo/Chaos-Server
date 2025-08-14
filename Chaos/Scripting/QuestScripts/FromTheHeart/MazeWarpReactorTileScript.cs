using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class MazeWarpReactorTileScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public MazeWarpReactorTileScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    public override void OnWalkedOn(Creature source)
    {
        if (source is not Aisling aisling) return;
        
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(aisling);
        if (questStatus != FromTheHeartQuestStatus.SpokenToMarlinAgain) return;
        var randomItem = ItemFactory.Create("strangeStone");
        var template = DialogFactory.Create("maze_secret_warp", randomItem);
        template.Display(aisling);
    }
}