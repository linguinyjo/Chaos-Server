using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.GeneralsOfDarkness;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts.LouresScripts;

public class ThibaultScript: DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly IItemFactory ItemFactory;
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public ThibaultScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory, ISimpleCache simpleCache)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isGeneralsQuestAvailable = GeneralsOfDarknessQuestHelper.IsQuestAvailable(source);
        if (isGeneralsQuestAvailable)
        {
            Subject.AddOption("Generals of Darkness", "thibault_generals_quest_initial");
        }
        var questStatus = GeneralsOfDarknessQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case GeneralsOfDarknessQuestStatus.ReturnedToEdric:
            case GeneralsOfDarknessQuestStatus.SlayTheGenerals:
                Subject.AddOption("Generals of Darkness", "thibault_report_from_edric");
                break;
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {}
}
