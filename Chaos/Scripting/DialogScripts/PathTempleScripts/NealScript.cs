using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.PathTempleScripts;

public class NealScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public NealScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isWarrior = source.HasClass(BaseClass.Warrior);
        var isPeasant = source.HasOnlyPeasant();
        if (isPeasant)
        {
            Subject.AddOption("I want to become a warrior", "neal_become_a_warrior");
        }
        
        Subject.AddOption("Tell me about the path of the warrior", "neal_warrior_info");

        if (source.UserStatSheet.Level == 50 && isWarrior)
        {
            Subject.AddOption("I want to dedicate myself to the path", "neal_advanced_class");
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
