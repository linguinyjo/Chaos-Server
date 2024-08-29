using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

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
    public override void OnDisplaying(Aisling source) {}

    public override void OnDisplayed(Aisling source)
    {
        var stick = ItemFactory.Create("stick");
        var itemAdded = source.Inventory.TryAddToNextSlot(stick);
        if (itemAdded) TrainingQuestHelper.IncrementQuestStage(source);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
