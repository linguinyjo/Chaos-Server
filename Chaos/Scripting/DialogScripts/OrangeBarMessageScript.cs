using Chaos.Collections;
using Chaos.Common.Definitions;
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
    
    #region ScriptVars
    protected string? Message { get; init; }
    #endregion

    /// <inheritdoc />
    public OrangeBarMessageScript(Dialog subject) : base(subject) {}
    

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {}

    public override void OnDisplayed(Aisling source)
    {
        
    }
    

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (Message != null) source.SendOrangeBarMessage(Message);
    }
}