using System.Text.RegularExpressions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MerchantScripts.ShopScripts;
using Chaos.Scripting.MerchantScripts.ShopScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class BlueDugonVerbalScript : VerbalShopScriptBase
{
    private readonly BlueDugonQuestHelper BlueDugonQuestHelper = new();
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public BlueDugonVerbalScript(Merchant subject, ILogger<VerbalSellShopScript> logger, IDialogFactory dialogFactory,
        IItemFactory itemFactory)
        : base(subject, logger)
    {
        DialogFactory = dialogFactory;
        Merchant = subject;
        ItemFactory = itemFactory;
    }

    private Merchant Merchant { get; set; }

    /// <inheritdoc />
    public override void OnPublicMessage(Creature source, string message)
    {
        if (source is not Aisling aisling) return;
        var questStatus = BlueDugonQuestHelper.GetQuestStatus(aisling);
        if (questStatus != BlueDugonQuestStatus.MeditationCompleted) return;
        var match = DugonRegexCache.BLUE_DUGON_PATTERNS
            .Select(regex => regex.Match(message))
            .FirstOrDefault(x => x.Success);

        if (match is null) return;
        var item = ItemFactory.Create("blueDugon");
        var dugonGiven = aisling.Inventory.TryAddToNextSlot(item);
        if (!dugonGiven)
        {
            DisplayDialog(
                aisling,
                "Make sure you have room in your inventory.",
                "close"
            );
        }
        else
        {
            BlueDugonQuestHelper.CompleteQuest(aisling);
            DisplayDialog(
                aisling,
                "Well done. Achieving the Blue Dugon marks an important step on your path to mastering the martial arts.",
                "white_dugon_complete"
            );
        }
    }

    private void DisplayDialog(Aisling aisling, string message, string nextDialogKey)
    {
        var dialog = new Dialog(
            Merchant,
            DialogFactory,
            ChaosDialogType.Normal,
            message)
        {
            NextDialogKey = nextDialogKey
        };
        dialog.Display(aisling);
    }
}