using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.DevlinsIngredients;

public class DevlinsIngredientsQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public DevlinsIngredientsQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = DevlinsIngredientsQuestHelper.GetQuestStatus(source);
        if (questStatus is DevlinsIngredientsQuestStatus.FetchRawWax)
        {
            HandleFetchRawWax(source);
        } else if (questStatus is DevlinsIngredientsQuestStatus.FetchRawHoney)
        {
            HandleFetchRawHoney(source);
        } else if (questStatus is DevlinsIngredientsQuestStatus.OneFinalTask)
        {
            HandleOneFinalTask(source);
        } else if (questStatus is DevlinsIngredientsQuestStatus.ToShinewood)
        {
            HandleToShinewood(source);
        }
    }

    private void HandleFetchRawWax(Aisling source)
    {
        // check for raw wax
        var hasWax = source.Inventory.HasCountByTemplateKey("rawWax", 1);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "Well done! You've successfully gathered the raw wax. I can sense its latent magical properties. Now, for your next task: return to the East Woodland and collect raw honey from the same bees. Be warned, this will be more challenging as the honey can be quite rare.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("rawWax", 1);
            source.TryGiveGold(750);
            source.GiveExperience(750);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "I see you've returned, but without the raw wax. The bees of East Woodland can be challenging, but persistence is key. Return when you've gathered the wax.",
                nextKey: "Close"
            );
            newDialog.Display(source);
        }
    }
    
    private void HandleFetchRawHoney(Aisling source)
    {
        var hasWax = source.Inventory.HasCountByTemplateKey("rawHoney", 1);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "Wonderful! You managed to find some honey. I've been needing this for quite some time. Return to me in a short while, for I will have one final task for you.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("rawHoney", 1);
            source.TryGiveGold(1500);
            source.GiveExperience(1500);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "Ah, the honey proves elusive, does it? Do not be discouraged. The rarity of the honey makes it all the more valuable. Return once you have found some.",
                nextKey: "Close"
            );
            newDialog.Display(source);
        }
    }
    
    private void HandleOneFinalTask(Aisling source)
    {
        var newDialog = CreateDialog(
            text: "The last item I need is going to be far more challenging to come by. There is a place called Shinewood Forest far to the south of here.",
            nextKey: "devlin_to_shinewood_a"
        );
        newDialog.Display(source);
    }
    
    private void HandleToShinewood(Aisling source)
    {
        var hasWax = source.Inventory.HasCountByTemplateKey("beesWing", 5);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "Well done!",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("rawWax", 1);
            source.TryGiveGold(750);
            source.GiveExperience(750);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "I see you've returned, but without the wings.",
                nextKey: "Close"
            );
            newDialog.Display(source);
        }
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
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
