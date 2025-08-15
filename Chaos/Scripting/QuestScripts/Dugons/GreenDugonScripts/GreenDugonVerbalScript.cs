using System.Text.RegularExpressions;
using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MerchantScripts.ShopScripts;
using Chaos.Scripting.MerchantScripts.ShopScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;

public class GreenDugonVerbalScript : VerbalShopScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly GreenDugonQuestHelper GreenDugonQuestHelper = new();
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public GreenDugonVerbalScript(Merchant subject, ILogger<VerbalSellShopScript> logger, IDialogFactory dialogFactory,
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
        var questStatus = GreenDugonQuestHelper.GetQuestStatus(aisling);
        if (questStatus != GreenDugonQuestStatus.MeditationCompleted) return;
        var match = DugonRegexCache.GREEN_DUGON_PATTERNS
            .Select(regex => regex.Match(message))
            .FirstOrDefault(x => x.Success);

        if (match is null) return;
        var item = ItemFactory.Create("greenDugon");
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
            GreenDugonQuestHelper.CompleteQuest(aisling);
            DisplayDialog(
                aisling,
                "Well done. Achieving the Green Dugon marks an important step on your path to mastering the martial arts.",
                "white_dugon_complete"
            );
        }
    }

    private static Match? FindFirstMatch(string input, IEnumerable<Regex> patterns)
    {
        return patterns
            .Select(regex => regex.Match(input))
            .FirstOrDefault(x => x.Success);
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