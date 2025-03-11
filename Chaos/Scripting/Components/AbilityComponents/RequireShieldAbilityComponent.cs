using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct  RequireShieldAbilityComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IRequireShieldComponentOptions>();
        if (options.RequireShield is null or false) return true;
        var shield = context.SourceAisling?.Equipment[EquipmentSlot.Shield];
        return shield != null;
    }

    public interface IRequireShieldComponentOptions
    {
        bool? RequireShield { get; init; }
    }
}