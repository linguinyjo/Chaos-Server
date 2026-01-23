using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.FromTheHeart;
using Chaos.Scripting.QuestScripts.GeneralsOfDarkness;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts.LouresScripts;

public class JeanScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public JeanScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
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
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var status = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (status is FromTheHeartQuestStatus.None or FromTheHeartQuestStatus.Completed) return;
        // new dialog here
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Menu,
            "The transformation of Deoch's hot intoxication into Glioca's moonlit compassion reminds me of sweet Bella who used to visit me."
        );
        dialog.AddOption("Where is Bella?", "jean_where_is_bella");
        dialog.AddOption("Do you love Bella?", "jean_do_you_love_bella");
        dialog.Display(source);
    }
}