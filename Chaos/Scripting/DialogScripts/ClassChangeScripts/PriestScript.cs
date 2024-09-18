using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class PriestScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public PriestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isPriest = source.HasClass(BaseClass.Priest);
        var isPeasant = source.HasClass(BaseClass.Peasant);
        switch (isPriest)
        {
            case true when source.UserStatSheet.Level > 10:
                Subject.AddOption("I want to become stronger", "erin_advanced_class");
                break;
            case true:
            {
                var newDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "You walk the path of the Priest already my child.")
                {
                    NextDialogKey = "Close"
                };
                newDialog.Display(source);
                break;
            }
            default:
            {
                if (!isPeasant)
                {
                    // Some other class 
                    var newDialog = new Dialog(
                        Dialog.DialogSource,
                        DialogFactory,
                        ChaosDialogType.Normal,
                        $"You have chosen your path already.")
                    {
                        NextDialogKey = "Close"
                    };
                    newDialog.Display(source);
                }
                break;
            }
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}