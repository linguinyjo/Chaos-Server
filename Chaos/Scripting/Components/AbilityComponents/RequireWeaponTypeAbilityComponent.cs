using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct  RequireWeaponTypeAbilityComponent : IConditionalComponent
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IRequireWeaponTypeComponentOptions>();
        if (options.WeaponCategory == null) return true;
        var weapon = context.SourceAisling?.Equipment[EquipmentSlot.Weapon];
        return weapon?.Template.Category == options.WeaponCategory;
    }

    public interface IRequireWeaponTypeComponentOptions
    {
        string? WeaponCategory { get; init; }
    }
}