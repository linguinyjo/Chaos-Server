using System.Text.RegularExpressions;
using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MerchantScripts.ShopScripts;
using Chaos.Scripting.MerchantScripts.ShopScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PorteForest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;

public class WhiteDugonVerbalScript : VerbalShopScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private Merchant Merchant { get; set; }
    private readonly IItemFactory ItemFactory;
    private readonly WhiteDugonQuestHelper WhiteDugonQuestHelper = new();

    /// <inheritdoc />
    public WhiteDugonVerbalScript(Merchant subject, ILogger<VerbalSellShopScript> logger,  IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject, logger)
    {
        DialogFactory = dialogFactory;
        Merchant = subject;
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnPublicMessage(Creature source, string message)
    {
        if (source is not Aisling aisling) return;
        var questStatus = WhiteDugonQuestHelper.GetQuestStatus(aisling);
        if (questStatus != WhiteDugonQuestStatus.MeditationCompleted) return;
        var match = DugonRegexCache.WHITE_DUGON_PATTERNS
            .Select(regex => regex.Match(message))
            .FirstOrDefault(x => x.Success);

        if (match is null) return;
        var item = ItemFactory.Create("whiteDugon");
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
            WhiteDugonQuestHelper.CompleteQuest(aisling);
            DisplayDialog(
                aisling,
                "Well done. The White Dugon symbolizes the beginning of your journey. Wear it proudly, and remember that true strength flows from both body and mind.",
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
