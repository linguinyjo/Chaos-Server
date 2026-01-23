using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;
using Chaos.Scripting.QuestScripts.Dugons.GreenDugonScripts;
using Chaos.Scripting.QuestScripts.Dugons.WhiteDugonScripts;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public class KyrosScript : DialogScriptBase
{
    private readonly BlueDugonQuestHelper _blueDugonQuestHelper = new();
    private readonly GreenDugonQuestHelper _greenDugonQuestHelper = new();
    private readonly WhiteDugonQuestHelper _whiteDugonQuestHelper = new();
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public KyrosScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        // which option to show? 
        if (!source.HasClass(BaseClass.Monk)) return;

        var currentDugon = source.Trackers.Enums.TryGetValue<Dugon>(out var status) ? status : Dugon.None;

        switch (currentDugon)
        {
            case Dugon.None:
                if (_whiteDugonQuestHelper.IsEligible(source))
                {
                    Subject.AddOption("White Dugon", "white_dugon_1");
                }

                return;
            case Dugon.White:
                if (_greenDugonQuestHelper.IsEligible(source))
                {
                    Subject.AddOption("Green Dugon", "green_dugon_1");
                }

                return;
            case Dugon.Green:
                if (_blueDugonQuestHelper.IsEligible(source))
                {
                    Subject.AddOption("Blue Dugon", "blue_dugon_1");
                }

                return;
        }
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}