using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.AJourneyToSuomi;
using Chaos.Scripting.QuestScripts.DevlinsIngredients;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class OonaScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public OonaScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isAvailable = AJourneyToSuomiQuestHelper.IsQuestAvailable(source);
        if (AJourneyToSuomiQuestHelper.GetQuestStatus(source) is AJourneyToSuomiQuestStatus.FetchFruitShipment)
        {
            Subject.AddOption("How do I get to Suomi?", "oona_how_to_get_to_suomi");
        }
        else if (isAvailable)
        {
            Subject.AddOption("A Journey to Suomi", "Oona_a_journey_to_suomi_quest_a");
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
