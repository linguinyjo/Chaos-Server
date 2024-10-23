using Chaos.Models.Abstractions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.NLog.Logging.Definitions;
using Chaos.NLog.Logging.Extensions;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Utilities;

namespace Chaos.Scripting.DialogScripts.ShopScripts;

public class RepairShopScript : DialogScriptBase
{
    private readonly ILogger<RepairShopScript> Logger;
    private readonly ISellShopSource SellShopSource;

    /// <inheritdoc />
    public RepairShopScript(Dialog subject, ILogger<RepairShopScript> logger)
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
            case "generic_repairshop_initial":
            {
                OnDisplayingInitial(source);

                break;
            }
            case "generic_repairshop_confirmation":
            {
                OnDisplayingConfirmation(source);

                break;
            }
            case "generic_repairshop_accepted":
            {
                OnDisplayingAccepted(source);

                break;
            }
        }
    }

    private void OnDisplayingAccepted(Aisling source)
    {
        if (!TryFetchArgs<byte>(out var slot)
            || !source.Inventory.TryGetObject(slot, out var item))
        {
            Subject.ReplyToUnknownInput(source);

            return;
        }

        if (item.Template.MaxDurability == null || item.CurrentDurability == null) return;
        
        // var totalRepairValue = (int)Math.Round((item.Template.BuyCost * (
        //     1 - ((decimal)item.CurrentDurability.Value / item.Template.MaxDurability.Value)) * 0.1m));
        
        var repairItemResult = ComplexActionHelper.RepairItem(source, slot);

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
                      .WithProperty(item)
                      .LogInformation(
                          "Aisling {@AislingName} repaired {@ItemName} to merchant {@MerchantName}",
                          source.Name,
                          item.DisplayName,
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

    private void OnDisplayingConfirmation(Aisling source)
    {
        if (!TryFetchArgs<byte>(out var slot)
            || !source.Inventory.TryGetObject(slot, out var item))
        {
            Subject.ReplyToUnknownInput(source);

            return;
        }

        var repairCost = ComplexActionHelper.CalculateItemRepairCost(item);
        if (repairCost == null) return;
        Subject.InjectTextParameters(item.DisplayName, repairCost);
    }

    private void OnDisplayingInitial(Aisling source)
    {
        Subject.Slots = source.Inventory
            .Where(item => item.CurrentDurability < item.Template.MaxDurability)
            .Select(item => item.Slot)
            .ToList();
    }
        
}