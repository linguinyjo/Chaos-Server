using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct SetDefenseElementAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<ISetDefenseElementComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            if(target.StatSheet.DefenseElement is Element.Darkness or Element.Holy) continue;
            target.StatSheet.SetDefenseElement(options.Element);
        }
    }
    
    public interface ISetDefenseElementComponentOptions
    {
        Element Element { get; init; }
    }
}
