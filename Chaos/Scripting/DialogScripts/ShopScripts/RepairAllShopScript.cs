using Chaos.Models.Abstractions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.NLog.Logging.Definitions;
using Chaos.NLog.Logging.Extensions;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Utilities;

namespace Chaos.Scripting.DialogScripts.ShopScripts;

public class RepairAllShopScript : DialogScriptBase
{
    private readonly ILogger<RepairShopScript> Logger;
    private readonly ISellShopSource SellShopSource;

    /// <inheritdoc />
    public RepairAllShopScript(Dialog subject, ILogger<RepairShopScript> logger)
        : base(subject)
    {
        Logger = logger;
        SellShopSource = (ISellShopSource)subject.DialogSource;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
      
        switch (Subject.Template.TemplateKey.ToLower())
        {
            
            case "generic_repairallshop_initial":
            {
                OnDisplayingInitial(source);
                break;
            }
           
            case "generic_repairallshop_accepted":
            {
                OnDisplayingAccepted(source);
                break;
            }
        }
    }

    private void OnDisplayingAccepted(Aisling source)
    {
        
        var repairItemResult = ComplexActionHelper.RepairAllItems(source);

        switch (repairItemResult)
        {
            case ComplexActionHelper.RepairItemResult.Success:
                Logger.WithTopics(
                          Topics.Entities.Aisling,
                          Topics.Entities.Item,
                          Topics.Entities.Gold,
                          Topics.Actions.Sell)
                      .WithProperty(Subject)
                      .WithProperty(Subject.DialogSource)
                      .WithProperty(source)
                      .LogInformation(
                          "Aisling {@AislingName} repaired all items to merchant {@MerchantName}",
                          source.Name,
                          SellShopSource.Name);

                return;
            case ComplexActionHelper.RepairItemResult.NotEnoughGold:
                Subject.Reply(source, "You don't have enough gold.", "generic_sellshop_initial");
                return;

            case ComplexActionHelper.RepairItemResult.BadInput:
                Subject.ReplyToUnknownInput(source);
                return;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnDisplayingInitial(Aisling source)
    {
        var repairCost = ComplexActionHelper.CalculateAllRepairCost(source);
        if (repairCost == null) return;
        Subject.InjectTextParameters(repairCost);
    }
}