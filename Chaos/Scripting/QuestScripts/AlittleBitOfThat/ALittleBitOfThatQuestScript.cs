using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.DevlinsIngredients;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.AlittleBitOfThat;

public class ALittleBitOfThatQuestScript : DialogScriptBase
{
    private static readonly string[] RottenItems =
    [
        "rottenApple",
        "rottenGrapes",
        "rottenTomato",
        "rottenCherry",
        "mouldyBaguette"
    ];

    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public ALittleBitOfThatQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        // Check what items the player has and count them
        var itemsFound = new List<(string itemKey, int count)>();
        var totalGoldValue = 0;

        foreach (var itemKey in RottenItems)
        {
            var count = source.Inventory.CountOfByTemplateKey(itemKey);
            if (count <= 0) continue;
            itemsFound.Add((itemKey, count));
            totalGoldValue += count * 500; // Each item worth 500 gold
        }

        // If no items found, show "nothing" dialog
        if (itemsFound.Count == 0)
        {
            var newDialog = CreateDialog(
                text:
                "Hmmmm, looks like you don't have any rotten items I'm interested in. Come back if you happen to find anything. The more rotten the better!",
                nextKey: "Close"
            );
            newDialog.Display(source);
            return;
        }

        foreach (var (itemKey, count) in itemsFound)
        {
            source.Inventory.RemoveQuantityByTemplateKey(itemKey, count);
        }

        source.TryGiveGold(totalGoldValue);

        var successDialog = CreateDialog(
            text:
            $"Thanks! I've given you {totalGoldValue} gold for your rotten items. Please bring me more if you can.",
            nextKey: "Close"
        );
        successDialog.Display(source);
    }

    private Dialog CreateDialog(string text, string nextKey)
    {
        return new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            text)
        {
            NextDialogKey = nextKey
        };
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}