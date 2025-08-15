using System.Text.RegularExpressions;
using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MerchantScripts.ShopScripts;
using Chaos.Scripting.MerchantScripts.ShopScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class VerbalQuestScript : VerbalShopScriptBase
{
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public VerbalQuestScript(Merchant subject, ILogger<VerbalSellShopScript> logger, IDialogFactory dialogFactory)
        : base(subject, logger)
    {
        DialogFactory = dialogFactory;
        Merchant = subject;
    }

    private Merchant Merchant { get; set; }


    /// <inheritdoc />
    public override void OnPublicMessage(Creature source, string message)
    {
        if (source is not Aisling aisling) return;
        var handler = GetMessageHandler(aisling);
        handler?.Invoke(aisling, message);
    }

    private MessageHandler? GetMessageHandler(Aisling aisling)
    {
        var questStatus = PorteForestQuestHelper.GetQuestStatus(aisling);

        return (Merchant.Template.TemplateKey, questStatus) switch
        {
            ("torbjorn", PorteForestQuestStatus.Started) => HandleTorbjornInitial,
            ("torbjorn", PorteForestQuestStatus.SpokenToTorbjorn) => HandleTorbjornReturnWithRoots,
            ("torbjorn", PorteForestQuestStatus.DeliveredTheRoots) => HandleTorbjornReturnForInformation,
            ("bertil", _) => HandleBertilTarp,
            _ => null
        };
    }

    private void HandleTorbjornInitial(Aisling aisling, string message)
    {
        var porteForestMatch = FindFirstMatch(message, PorteForestRegexCache.PORTE_FOREST_PATTERNS);
        if (porteForestMatch is null) return;

        DisplayDialog(aisling,
            "Well, well... I've not heard those two words spoken out loud in quite some time aisling.",
            "torbjorn_porte_a");
    }

    private void HandleTorbjornReturnWithRoots(Aisling aisling, string message)
    {
        DisplayDialog(aisling,
            "Ah, so you've returned...",
            "torbjorn_porte_trent_roots");
    }

    private void HandleTorbjornReturnForInformation(Aisling aisling, string message)
    {
        DisplayDialog(aisling,
            "Ah, so you've returned...",
            "torbjorn_porte_information");
    }

    private void HandleBertilTarp(Aisling aisling, string message)
    {
        var tarpMatch = FindFirstMatch(message, PorteForestRegexCache.TARP_PATTERNS);
        if (tarpMatch is null) return;

        if (PorteForestQuestHelper.IsElligibleToMakeTarp(aisling))
        {
            DisplayDialog(aisling,
                "So you're interested in one of my special Tarps are you?",
                "bertil_porte_tarp_a");
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

    private delegate void MessageHandler(Aisling aisling, string message);
}