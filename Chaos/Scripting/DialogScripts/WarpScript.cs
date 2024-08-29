using Chaos.Collections;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class WarpScript : ConfigurableDialogScriptBase
{
    private readonly ISimpleCache SimpleCache;
    
    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    #endregion
    
    /// <inheritdoc />
    public WarpScript(Dialog subject, ISimpleCache simpleCache)
        : base(subject) => SimpleCache = simpleCache;
    
    /// <inheritdoc />
    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        source.TraverseMap(targetMap, Destination);
    }
}