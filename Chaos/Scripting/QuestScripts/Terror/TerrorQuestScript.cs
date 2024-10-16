using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public TerrorQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = TerrorQuestHelper.GetQuestStatus(source);
        if (questStatus is TerrorQuestStatus.None or TerrorQuestStatus.GardenStarted)
        {
            var goldTaken = source.TryTakeGold(2000);
            if (!goldTaken)
            {
                Subject.Close(source);
                return;
            }
            if(questStatus is TerrorQuestStatus.None)
            {
                TerrorQuestHelper.StartQuest(source);
            }
        } else if (questStatus is TerrorQuestStatus.GardenSlain)
        {
            TerrorQuestHelper.IncrementQuestStage(source);
        }  else if (questStatus is TerrorQuestStatus.GardenCompleted or TerrorQuestStatus.AlleyStarted)
        {
            var goldTaken = source.TryTakeGold(5000);
            if (!goldTaken) return;
            if(questStatus is TerrorQuestStatus.GardenCompleted)
            {
                TerrorQuestHelper.IncrementQuestStage(source);
            }
        } 
        else if (questStatus is TerrorQuestStatus.AlleySlain)
        {
            TerrorQuestHelper.IncrementQuestStage(source);
        }
        
        else if (questStatus is TerrorQuestStatus.AlleyCompleted or TerrorQuestStatus.CryptStarted)
        {
            var goldTaken = source.TryTakeGold(10000);
            if (!goldTaken) return;
            if(questStatus is TerrorQuestStatus.AlleyCompleted)
            {
                TerrorQuestHelper.IncrementQuestStage(source);
            }
        }  else if (questStatus is TerrorQuestStatus.CryptSlain)
        {
            TerrorQuestHelper.IncrementQuestStage(source);
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
     
    }
}
