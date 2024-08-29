using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class WarriorScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public WarriorScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isWarrior = source.HasClass(BaseClass.Warrior);
        var isPeasant = source.HasClass(BaseClass.Peasant);
        switch (isWarrior)
        {
            case true when source.UserStatSheet.Level > 10:
                Subject.AddOption("I want to become stronger", "neal_advanced_class");
                break;
            case true:
            {
                var newDialog = new Dialog(
                    source,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "You are already a warrior, come back when you are stronger")
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
                        source,
                        DialogFactory,
                        ChaosDialogType.Normal,
                        $"You already have a class. Becoming a warrior is no longer an option for you. Begone from here {source.UserStatSheet.BaseClass}!")
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