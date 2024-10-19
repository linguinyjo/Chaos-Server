using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class BergScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public BergScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (!source.HasClass(BaseClass.Monk)) return;
        var formStatus = source.Trackers.Enums.TryGetValue<MonkFormType>(out var status) ? status : MonkFormType.None;
        if (formStatus != MonkFormType.None)
        {
            HandlePlayerAlreadyLearned(source, formStatus);
            return;
        }
        
        Subject.AddOption("Draco Form", "draco_form_initial");
        Subject.AddOption("Kelberoth Form", "kelberoth_form_initial");
        Subject.AddOption("White Bat Form", "white_bat_form_initial");
        Subject.AddOption("Scorpion Form", "scorpion_form_initial");
    }

    private void HandlePlayerAlreadyLearned(Aisling source, MonkFormType formStatus)
    {
        Subject.AddOption("Unlearn Current Form", "unlearn_form");
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
