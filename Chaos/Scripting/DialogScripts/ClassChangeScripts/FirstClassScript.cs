using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class FirstClassScript:  ConfigurableDialogScriptBase
{
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    // Dictionary to store dialog messages and class change keys
    private static readonly Dictionary<BaseClass, (string alreadyClassText, string rejectionText, string classChangeKey)> DialogTexts =
        new()
        {
            { BaseClass.Rogue,  ("You are already a rogue, come back when you have become a master of the shadows", 
                "Hah! The loud stomp of your feet gives you away, aisling. You have no business here - begone now.",
                "keefe_class_change") },

            { BaseClass.Warrior, (
                "You are already a warrior, come back when you are stronger", 
                "You already have a class. Becoming a warrior is no longer an option for you. Begone from here.",
                "neal_class_change") },

            { BaseClass.Wizard,    (
                "You are already a wizard, come back when you have acquired greater knowledge of the elements.", 
                "It is too late for you. Mastering the elements is no longer an option.",
                "logan_class_change") },

            { BaseClass.Priest,  (
                "You walk the path of the Priest already my child.", 
                "You have chosen your path already.",
                "erin_class_change") },

            { BaseClass.Monk, (
                "You are already a monk, come back when you have acquired greater harmony and balance", 
                "A life of harmony and balance is no longer your path Aisling. Leave from here and be at peace with the path you have chosen to walk.",
                "donnan_class_change") }
        };
    
    /// <inheritdoc />
    public FirstClassScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)  {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 
    
    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var classType = (BaseClass)Class; 
        var isCurrentClass = source.HasClass(classType);
        var isPeasant = source.HasOnlyPeasant();

        if (isCurrentClass)
        {
            var newDialog = new Dialog(
                Dialog.DialogSource,
                DialogFactory,
                ChaosDialogType.Normal,
                DialogTexts[classType].alreadyClassText)
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
            return;
        }

        if (isPeasant)
        {
            Subject.AddOption("I am prepared", DialogTexts[classType].classChangeKey);
        }
        else
        {
            var newDialog = new Dialog(
                Dialog.DialogSource,
                DialogFactory,
                ChaosDialogType.Normal,
                DialogTexts[classType].rejectionText)
            {
                NextDialogKey = "Close"
            };
            newDialog.Display(source);
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}