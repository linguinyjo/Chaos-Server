using Chaos.Collections.Common;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Scripting.SpellScripts.WaystoneScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class WaystoneScript : ConfigurableDialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public WaystoneScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        Dialog = subject;
        DialogFactory = dialogFactory;
    }

    #region ScriptVars

    protected byte Waystone { get; init; }

    #endregion

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var isRequiredClass = source.UserStatSheet.BaseClass is BaseClass.Druid or BaseClass.Elementalist;
        if (!isRequiredClass)
        {
            HandleWrongClass(source);
            return;
        }

        var currentWaystone = (Waystone)Waystone;

        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Menu,
            "*The waystone stands before you. You can sense its latent powers hidden behind the intricate markings...*")
        {
            Options =
            [
                new DialogOption
                {
                    OptionText = "Commit the markings to memory",
                    DialogKey = "waystone_save"
                }
            ],
            MenuArgs = new ArgumentCollection([currentWaystone.ToString()])
        };
        dialog.Display(source);
    }

    private void HandleWrongClass(Aisling source)
    {
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            "* The mysteries of this stone lie far beyond your comprehension... *")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}