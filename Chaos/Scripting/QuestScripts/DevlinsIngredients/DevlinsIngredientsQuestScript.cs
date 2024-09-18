using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;
using Chaos.Time;

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
        var hasWax = source.Inventory.HasCountByTemplateKey("rawWax", 3);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "Well done! You've successfully gathered the raw wax. I can sense its magical properties. Now, I desperately need a Centipede gland for my research. You should be able to find one from the crypt in town.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("rawWax", 3);
            source.TryGiveGold(250);
            source.GiveExperience(250);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "I see you've returned, but without the raw wax. Return when you've gathered the wax.",
                nextKey: "Close"
            );
            newDialog.Display(source);
        }
    }
    
    private void HandleFetchRawHoney(Aisling source)
    {
        var hasWax = source.Inventory.HasCountByTemplateKey("centipedeGland", 1);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "Wonderful! You managed to find the centipede gland. And one in such excellent condition too. Return to me in a moment, I have one final task for you.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("centipedeGland", 1);
            source.TryGiveGold(500);
            source.GiveExperience(250);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "Ah, the centipede gland proves elusive, does it? Please find me one as soon as possible.",
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
        var hasWax = source.Inventory.HasCountByTemplateKey("beeWing", 3);
        if (hasWax)
        {
            var newDialog = CreateDialog(
                text: "You did it! I must say I wasn't expecting to see you again Aisling. Here, take this gold for your trouble.",
                nextKey: "Close"
            );
            newDialog.Display(source);
            source.Inventory.RemoveQuantityByTemplateKey("beeWing", 3);
            source.TryGiveGold(2000);
            source.GiveExperience(250);
            DevlinsIngredientsQuestHelper.IncrementQuestStage(source);
            var legendMark = new LegendMark(
                "Brought Devlin her ingredients",
                "devlinsIngredients", 
                MarkIcon.Victory,
                MarkColor.White,
                1,
                GameTime.Now);
            source.Legend.AddOrAccumulate(legendMark);
        }
        else
        {
            var newDialog = CreateDialog(
                text: "I see you've returned, but without the wings. Don't be fearful now Aisling. Bring me 3 wings from the bee's in Shinewood forest.",
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
