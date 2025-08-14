using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.FromTheHeart;
using Chaos.Scripting.QuestScripts.GeneralsOfDarkness;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts.LouresScripts;

public class MarlinScript: DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public MarlinScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
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
        var isFromTheHeartAvailable = FromTheHeartQuestHelper.IsQuestAvailable(source);
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (isFromTheHeartAvailable & questStatus == FromTheHeartQuestStatus.None || 
            questStatus == FromTheHeartQuestStatus.SpokenToMarlin || 
            questStatus == FromTheHeartQuestStatus.SpokenToJean)
        {
            Subject.AddOption("From the Heart", "marlin_from_the_heart_initial");
        }

        if (questStatus is FromTheHeartQuestStatus.SpokenToFaerie or FromTheHeartQuestStatus.SpokenToMarlinAgain)
        {
            Subject.AddOption("From the Heart", "marlin_did_you_give_bella_the_letter");
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {}
}
