using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.PathTempleScripts;

public class DonnanScript :  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public DonnanScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isMonk = source.HasClass(BaseClass.Monk);
        var isPeasant = source.HasOnlyPeasant();
        if (isPeasant)
        {
            Subject.AddOption("I want to become a monk", "donnan_become_a_monk");
        }
        
        Subject.AddOption("Tell me about the path of the monk", "donnan_monk_info");

        if (source.UserStatSheet.Level == 50 && isMonk)
        {
            Subject.AddOption("I want to dedicate myself to the path", "donnan_advanced_class");
        }
    }

    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
