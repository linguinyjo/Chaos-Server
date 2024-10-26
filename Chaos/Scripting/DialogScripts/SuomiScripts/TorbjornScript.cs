using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.AJourneyToSuomi;
using Chaos.Scripting.QuestScripts.PorteForest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class TorbjornScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public TorbjornScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        // var porteForestQuestStatus = PorteForestQuestHelper.GetQuestStatus(source);
        // if (porteForestQuestStatus == PorteForestQuestStatus.SpokenToTorbjorn)
        // {
        //     Subject.AddOption("Trent roots", "torbjorn_porte_roots");
        // }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
