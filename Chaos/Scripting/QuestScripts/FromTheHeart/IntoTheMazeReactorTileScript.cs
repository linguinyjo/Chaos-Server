using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class IntoTheMazeReactorTileScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public IntoTheMazeReactorTileScript(ReactorTile subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
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
        var stone = ItemFactory.Create("strangeStone");
        var template = DialogFactory.Create("into_the_maze_initial", stone);
        template.Display(aisling);
    }
}