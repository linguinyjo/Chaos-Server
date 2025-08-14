using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.DarkThings;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class RedScarfDialogScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;

    
    /// <inheritdoc />
    public RedScarfDialogScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {}
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (optionIndex is not 1) return;
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (questStatus != FromTheHeartQuestStatus.SpokenToJean) return;
        var randomItem = ItemFactory.Create("komadium");
        var template = DialogFactory.Create("red_scarf_wait", randomItem);
        template.Display(source);
    }
}
