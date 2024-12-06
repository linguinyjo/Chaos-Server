using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class RogueScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public RogueScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)  {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isMonk = source.HasClass(BaseClass.Monk);
        var isPeasant = source.HasClass(BaseClass.Peasant);
        switch (isMonk)
        {
            case true when source.UserStatSheet.Level > 10:
                Subject.AddOption("I want to become stronger", "keefe_advanced_class");
                break;
            case true:
            {
                var newDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "You are already a rogue, come back when you have become a master of the shadows")
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
                        $"Hah! The loud stomp of your feet gives you away, aisling. You have no business here - begone now.")
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