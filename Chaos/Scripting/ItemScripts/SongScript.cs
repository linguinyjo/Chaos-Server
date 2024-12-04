using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.ItemScripts;

public class SongScript : ConfigurableItemScriptBase,
                                        ConsumableAbilityComponent.IConsumableComponentOptions
{
    private readonly ISimpleCache SimpleCache;
    
    #region ScriptVars
    protected Location Destination { get; init; } = null!;
    #endregion

    /// <inheritdoc />
    public SongScript(Item subject, ISimpleCache simpleCache)
        : base(subject)
    {
        SourceScript = this;
        Slot = Subject.Slot;
        Item = Subject;
        ItemName = Subject.DisplayName;
        SimpleCache = simpleCache;
    }

    /// <inheritdoc />
    public override void OnUse(Aisling source)
    {
        new ComponentExecutor(source, source).WithOptions(this)
            .Execute<ConsumableAbilityComponent>();
        var targetMap = SimpleCache.Get<MapInstance>(Destination.Map);
        source.TraverseMap(targetMap, Destination);
    }

    #region ScriptVars
    public IScript SourceScript { get; init; }
    public string ItemName { get; init; }
    public byte Slot { get; init; }
    public Item Item { get; init; }
    #endregion
}