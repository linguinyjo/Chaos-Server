using Chaos.Common.Definitions;
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
    private Merchant Merchant { get; set; }
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public VerbalQuestScript(Merchant subject, ILogger<VerbalSellShopScript> logger,  IDialogFactory dialogFactory)
        : base(subject, logger)
    {
      
        DialogFactory = dialogFactory;
        Merchant = subject;
    }


    /// <inheritdoc />
    public override void OnPublicMessage(Creature source, string message)
    {
        if (source is not Aisling aisling) return;

        var questStatus = PorteForestQuestHelper.GetQuestStatus(aisling);
        var match = RegexCache.PORTE_FOREST_PATTERNS
                              .Select(regex => regex.Match(message))
                              .FirstOrDefault(x => x.Success);
        
        if (match is null) return;

       
        if (Merchant.Template.TemplateKey == "torbjorn")
        {
            if (questStatus == PorteForestQuestStatus.Started)
            {
                var newDialog = new Dialog(
                    Merchant,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "Well, well... I've not heard those two words spoken out loud in quite some time aisling.")
                {
                    NextDialogKey = "torbjorn_porte_a"
                };
                newDialog.Display(aisling);
                // aisling.Trackers.Enums.Set(PorteForestQuestStatus.SpokenToTorbjorn);
            } else if (questStatus == PorteForestQuestStatus.SpokenToTorbjorn)
            {
                var newDialog = new Dialog(
                    Merchant,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "Ah, so you've returned...")
                {
                    NextDialogKey = "torbjorn_porte_trent_roots"
                };
                newDialog.Display(aisling);
            }
            
        }
        else if (Merchant.Template.TemplateKey == "valdemar")
        {
            {
                var newDialog = new Dialog(
                    Merchant,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "im the armorer round here")
                {
                    NextDialogKey = ""
                };
                newDialog.Display(aisling);
            }
        }



    }
    
    private const string ColorPrefix = "{=";
    
    private static string FormatColor(string text)
    {
        const MessageColor color = MessageColor.Orange;
        const char colorCode = (char)color;
        return $"{ColorPrefix}{colorCode}{text}";
    }
}