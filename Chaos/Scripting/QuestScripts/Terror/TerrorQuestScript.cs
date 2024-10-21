using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorQuestScript:  DialogScriptBase
{

    /// <inheritdoc />
    public TerrorQuestScript(Dialog subject) : base(subject) {} 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = TerrorQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case TerrorQuestStatus.None or TerrorQuestStatus.GardenStarted:
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
                break;
            }
            
            case TerrorQuestStatus.GardenSlain:
                TerrorQuestHelper.IncrementQuestStage(source);
                break;
            
            case TerrorQuestStatus.GardenCompleted or TerrorQuestStatus.AlleyStarted:
            {
                var goldTaken = source.TryTakeGold(5000);
                if (!goldTaken) return;
                if(questStatus is TerrorQuestStatus.GardenCompleted)
                {
                    TerrorQuestHelper.IncrementQuestStage(source);
                }
                break;
            }
            
            case TerrorQuestStatus.AlleySlain:
                TerrorQuestHelper.IncrementQuestStage(source);
                break;
            
            case TerrorQuestStatus.AlleyCompleted or TerrorQuestStatus.CryptStarted:
            {
                var goldTaken = source.TryTakeGold(10000);
                if (!goldTaken) return;
                if(questStatus is TerrorQuestStatus.AlleyCompleted)
                {
                    TerrorQuestHelper.IncrementQuestStage(source);
                }

                break;
            }
            
            case TerrorQuestStatus.CryptSlain:
                TerrorQuestHelper.CompleteQuest(source);
                break;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
