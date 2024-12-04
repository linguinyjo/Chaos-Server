using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts;

public class EnchantScript : ConfigurableItemScriptBase,
                                        ConsumableAbilityComponent.IConsumableComponentOptions,
                                        EnchantWeaponComponent.IEnchantWeaponComponentOptions
{
    
    /// <inheritdoc />
    public EnchantScript(Item subject)
        : base(subject)
    {
        SourceScript = this;
        LevelCircle = subject.LevelCircle;
        ItemName = subject.DisplayName;
        Slot = subject.Slot;
        Item = subject;
    }

    /// <inheritdoc />
    public override void OnUse(Aisling source)
    {
        new ComponentExecutor(source, source).WithOptions(this)
            ?.ExecuteAndCheck<EnchantWeaponComponent>()
            ?.Execute<ConsumableAbilityComponent>();
    }

    #region ScriptVars
    public IScript SourceScript { get; init; }
    /// <inheritdoc />
    public LevelCircle LevelCircle { get; init; }
    public string ItemName { get; init; }
    public byte Slot { get; init; }
    public Item Item { get; init; }

    public bool CanResist { get; init; }

    #endregion
}