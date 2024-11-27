using Chaos.Collections;
using Chaos.Collections.Common;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.AislingScripts;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class ResurrectionScript : DialogScriptBase
{
    private readonly ISimpleCache SimpleCache;

    /// <inheritdoc />
    public ResurrectionScript(Dialog subject, ISimpleCache simpleCache)
        : base(subject)
        => SimpleCache = simpleCache;

    /// <inheritdoc />
    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var mapInstance = SimpleCache.Get<MapInstance>("milethInn");
        var destination = new Location("milethInn", 5, 10);
        source.IsDead = false;
        source.StatSheet.SetHp(1);
        source.TraverseMap(mapInstance, destination);
        source?.Refresh(true);
        source?.Display();
    }
}