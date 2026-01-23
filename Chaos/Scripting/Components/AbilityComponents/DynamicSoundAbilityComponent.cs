using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct DynamicSoundAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IDynamicSoundComponentOptions>();
        var points = vars.GetPoints();
        var weaponType = context.SourceAisling?.Equipment[EquipmentSlot.Weapon]?.Template.Category;

        switch (weaponType)
        {
            case "Secret":
            case "Bow":
                context.TargetMap.PlaySound(9, points.ToArray());
                break;
            default:
                if (!options.Sound.HasValue) return;
                context.TargetMap.PlaySound(options.Sound.Value, points.ToArray());
                break;
        }
    }

    public interface IDynamicSoundComponentOptions
    {
        byte? Sound { get; init; }
    }
}