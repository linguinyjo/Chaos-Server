using Chaos.Collections;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.DarkThings;
using Chaos.Services.Factories.Abstractions;
using Chaos.Services.Storage;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class FromTheHeartQuestScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;
    private readonly ISimpleCache SimpleCache;


    /// <inheritdoc />
    public FromTheHeartQuestScript(Dialog subject, IDialogFactory dialogFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        switch (Subject.Template.TemplateKey)
        {
            case "marlin_give_bella_the_message_2":
                HandleGiveMessageTemplate(source);
                break;
            case "jean_would_you_release_marlin":
                HandleReleaseMarlinTemplate(source);
                break;
            case "faerie_where_is_she_now":
                HandleFaerieTemplate(source);
                break;
            case "marlin_i_will_tell_her":
                HandleIWillTellHerTemplate(source);
                break;
        }
    }

    private static void HandleGiveMessageTemplate(Aisling source)
    {
        if (FromTheHeartQuestHelper.IsQuestAvailable(source))
        {
            FromTheHeartQuestHelper.StartQuest(source);
        }
    }

    private static void HandleReleaseMarlinTemplate(Aisling source)
    {
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (questStatus == FromTheHeartQuestStatus.SpokenToMarlin)
        {
            source.Trackers.Enums.Set(FromTheHeartQuestStatus.SpokenToJean);
        }
    }

    private static void HandleFaerieTemplate(Aisling source)
    {
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (questStatus == FromTheHeartQuestStatus.SpokenToJean)
        {
            source.Trackers.Enums.Set(FromTheHeartQuestStatus.SpokenToFaerie);
        }
    }

    private static void HandleIWillTellHerTemplate(Aisling source)
    {
        var questStatus = FromTheHeartQuestHelper.GetQuestStatus(source);
        if (questStatus == FromTheHeartQuestStatus.SpokenToFaerie)
        {
            source.Trackers.Enums.Set(FromTheHeartQuestStatus.SpokenToMarlinAgain);
        }
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        switch (Subject.Template.TemplateKey)
        {
            case "red_scarf_wait":
            {
                var hasItem = source.Inventory.CountOfByTemplateKey("komadium") > 0;
                if (!hasItem)
                {
                    Subject.Reply(source,
                        "You go to take some from your inventory and then realise that you have none.", "Close");
                    return;
                }

                source.Inventory.RemoveQuantityByTemplateKey("komadium", 1);
                return;
            }
            case "into_the_maze_initial":
                if (optionIndex is 2)
                {
                    var mapInstance = SimpleCache.Get<MapInstance>("chamber2West");
                    var destination = new Location("chamber2West", 13, 26);
                    source.TraverseMap(mapInstance, destination);
                    source.SendOrangeBarMessage("A large pulse sucks you in");
                }

                break;
            case "maze_secret_warp":
                if (optionIndex is 2)
                {
                    var mapInstance = SimpleCache.Get<MapInstance>("heartOfStone");
                    var destination = new Location("heartOfStone", 3, 3);
                    source.TraverseMap(mapInstance, destination);
                    source.SendOrangeBarMessage("You slip through on a pulse of soft light");
                }

                break;
        }
    }
}