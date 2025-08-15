using Chaos.DarkAges.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct RequireShieldAbilityComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IRequireShieldComponentOptions>();
        if (options.RequireShield is null or false) return true;
        var shield = context.SourceAisling?.Equipment[EquipmentSlot.Shield];
        if (shield != null) return true;
        context.SourceAisling?.SendOrangeBarMessage("This ability requires a shield");
        return false;
    }

    public interface IRequireShieldComponentOptions
    {
        bool? RequireShield { get; init; }
    }
}