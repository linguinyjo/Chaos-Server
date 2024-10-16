using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class MonkScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public MonkScript(Dialog subject, IDialogFactory dialogFactory)
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
                Subject.AddOption("I want to become stronger", "donnan_advanced_class");
                break;
            case true:
            {
                var newDialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "You are already a monk, come back when you have acquired greater harmony and balance")
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
                        $"A life of harmony and balance is no longer your path Aisling. Leave from here and be at peace with the path you have chosen to walk.")
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