using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class UnlearnFormScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    private readonly ISkillFactory SkillFactory;
    private readonly ISpellFactory SpellFactory;

    /// <inheritdoc />
    public UnlearnFormScript(Dialog subject, IDialogFactory dialogFactory, ISkillFactory skillFactory, ISpellFactory spellFactory)
        : base(subject)
    {
        Dialog = subject;
        DialogFactory = dialogFactory;
        SkillFactory = skillFactory;
        SpellFactory = spellFactory;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        // Check stats
        var formStatus = source.Trackers.Enums.TryGetValue<MonkFormType>(out var status) ? status : MonkFormType.None;

        if (formStatus == MonkFormType.Draco)
        {
            source.SpellBook.RemoveByTemplateKey("dracoStance");
            source.SkillBook.RemoveByTemplateKey("dracoTailKick");

        }
       
        source.Trackers.Enums.Set(MonkFormType.None);
        
        var newDialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            "You have forgotten your chosen form. Please come back if you wish to learn another form.")
        {
            NextDialogKey = "Close"
        };
        newDialog.Display(source);
    }
    
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
