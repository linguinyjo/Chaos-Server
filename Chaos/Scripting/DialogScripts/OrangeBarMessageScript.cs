using Chaos.Collections;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.DialogScripts;

public class OrangeBarMessageScript : ConfigurableDialogScriptBase
{
    /// <inheritdoc />
    public OrangeBarMessageScript(Dialog subject) : base(subject)
    {
    }

    #region ScriptVars

    protected string? Message { get; init; }

    #endregion


    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
    }

    public override void OnDisplayed(Aisling source)
    {
    }


    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (Message != null) source.SendOrangeBarMessage(Message);
    }
}