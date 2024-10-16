using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class WizardScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public WizardScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isWizard = source.HasClass(BaseClass.Wizard);
        var isPeasant = source.HasClass(BaseClass.Peasant);
        switch (isWizard)
        {
            case true when source.UserStatSheet.Level > 10:
                Subject.AddOption("I want to become stronger", "logan_advanced_class");
                break;
            case true:
            {
                var newDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "You are already a wizard, come back when you have acquired greater knowledge of the elements!")
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
                        $"It is too late for regret young {source.UserStatSheet.BaseClass}. Mastering the elements is no longer an option for you.")
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