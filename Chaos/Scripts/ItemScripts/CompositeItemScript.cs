using System.Runtime.InteropServices;
using Chaos.Containers;
using Chaos.Objects.World;
using Chaos.Objects.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripts.ItemScripts.Abstractions;

namespace Chaos.Scripts.ItemScripts;

/// <summary>
///     DO NOT EDIT THIS SCRIPT
/// </summary>
public class CompositeItemScript : CompositeScriptBase<IItemScript>, IItemScript
{
    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public bool CanUse(Aisling source)
    {
        var canUse = true;

        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            canUse &= component.CanUse(source);

        return canUse;
    }

    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public virtual void OnDropped(Creature source, MapInstance mapInstance)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.OnDropped(source, mapInstance);
    }

    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public virtual void OnEquipped(Aisling aisling)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.OnEquipped(aisling);
    }

    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public virtual void OnPickup(Aisling aisling)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.OnPickup(aisling);
    }

    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public virtual void OnUnEquipped(Aisling aisling)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.OnUnEquipped(aisling);
    }

    /// <summary>
    ///     DO NOT EDIT THIS SCRIPT
    /// </summary>
    public virtual void OnUse(Aisling source)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.OnUse(source);
    }

    /// <inheritdoc />
    public void Update(TimeSpan delta)
    {
        foreach (ref var component in CollectionsMarshal.AsSpan(Components))
            component.Update(delta);
    }
}