using Chaos.Common.Definitions;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.QuestScripts.TrainingQuest;

public class VorlofTrainingQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    /// <inheritdoc />
    public VorlofTrainingQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        
    }

    public override void OnDisplayed(Aisling source)
    {
        var trainingQuestStatus = TrainingQuestHelper.GetQuestStatus(source);
        if (trainingQuestStatus is TrainingQuestStatus.SpokenToRiona)
        {
            var stick = ItemFactory.Create("stick");
            var itemAdded = source.Inventory.TryAddToNextSlot(stick);
            if (itemAdded) TrainingQuestHelper.IncrementQuestStage(source);
        }  else if (trainingQuestStatus is TrainingQuestStatus.CompletedTorrencesRequest)
        {
            TrainingQuestHelper.IncrementQuestStage(source);
            source.GiveExperience(1000);
            var legendMark = new LegendMark(
                "Completed Vorlof's training",
                "TrainingQuest",
                MarkIcon.Victory,
                MarkColor.White,
                1,
                GameTime.Now);
            source.Legend.AddOrAccumulate(legendMark);
        }
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
