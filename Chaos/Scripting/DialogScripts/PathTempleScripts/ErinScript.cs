using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.PathTempleScripts;

public class ErinScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public ErinScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isPriest = source.HasClass(BaseClass.Priest);
        var isPeasant = source.HasOnlyPeasant();
        if (isPeasant)
        {
            Subject.AddOption("I want to become a priest", "erin_become_a_priest");
        }
        
        Subject.AddOption("Tell me about the path of the priest", "erin_priest_info");

        if (source.UserStatSheet.Level == 50 && isPriest)
        {
            Subject.AddOption("I want to dedicate myself to the path", "erin_advanced_class");
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
