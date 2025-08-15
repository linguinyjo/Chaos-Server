using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Humanizer;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct DynamicBodyAnimationAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IDynamicBodyAnimationComponentOptions>();
        var weaponType = context.SourceAisling?.Equipment[EquipmentSlot.Weapon]?.Template.Category;

        switch (weaponType)
        {
            case not null when weaponType == WeaponCategory.Bow.ToString():
                context.Source.AnimateBody(BodyAnimation.JumpAttack, options.AnimationSpeed ?? 25);
                break;
            case not null when weaponType == WeaponCategory.TwoHanded.ToString():
                context.Source.AnimateBody(BodyAnimation.TwoHandAtk, options.AnimationSpeed ?? 25);
                break;
            default:
                context.Source.AnimateBody(options.BodyAnimation, options.AnimationSpeed ?? 25);
                break;
        }
    }

    public interface IDynamicBodyAnimationComponentOptions
    {
        ushort? AnimationSpeed { get; init; }
        BodyAnimation BodyAnimation { get; init; }
    }
}