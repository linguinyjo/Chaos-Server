using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.PathTempleScripts;

public class LoganScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public LoganScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isWizard = source.HasClass(BaseClass.Wizard);
        var isPeasant = source.HasOnlyPeasant();
        if (isPeasant)
        {
            Subject.AddOption("I want to become a wizard", "logan_become_a_wizard");
        }
        
        Subject.AddOption("Tell me about the path of the wizard", "logan_wizard_info");

        if (source.UserStatSheet.Level == 50 && isWizard)
        {
            Subject.AddOption("I want to dedicate myself to the path", "logan_advanced_class");
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
