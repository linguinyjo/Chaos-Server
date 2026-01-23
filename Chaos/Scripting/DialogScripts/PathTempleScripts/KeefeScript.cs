using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.PathTempleScripts;

public class KeefeScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public KeefeScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isRogue = source.HasClass(BaseClass.Rogue);
        var isPeasant = source.HasOnlyPeasant();
        if (isPeasant)
        {
            Subject.AddOption("I want to become a rogue", "keefe_become_a_rogue");
        }

        Subject.AddOption("Tell me about the path of the rogue", "keefe_rogue_info");

        if (source.UserStatSheet.Level == 50 && isRogue)
        {
            Subject.AddOption("I want to dedicate myself to the path", "keefe_advanced_class");
        }
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}