using Chaos.DarkAges.Definitions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts;

public class EnchantArmorScript : ConfigurableItemScriptBase,
    ConsumableAbilityComponent.IConsumableComponentOptions,
    EnchantArmorComponent.IEnchantArmorComponentOptions
{
    /// <inheritdoc />
    public EnchantArmorScript(Item subject)
        : base(subject)
    {
        LevelCircle = subject.LevelCircle;
        ItemName = subject.DisplayName;
        Slot = subject.Slot;
        Item = subject;
    }

    /// <inheritdoc />
    public override void OnUse(Aisling source)
    {
        new ComponentExecutor(source, source).WithOptions(this)
            ?.ExecuteAndCheck<EnchantArmorComponent>()
            ?.Execute<ConsumableAbilityComponent>();
    }

    #region ScriptVars

    public LevelCircle LevelCircle { get; init; }
    public string ItemName { get; init; }
    public byte Slot { get; init; }
    public Item Item { get; init; }

    #endregion
}