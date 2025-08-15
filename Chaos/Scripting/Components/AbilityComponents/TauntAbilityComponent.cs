using Chaos.DarkAges.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct TauntAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<ITauntComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            switch (target)
            {
                case Aisling aisling:
                    break;
                case Monster monster:
                    monster.AggroList.AddAggro(context.Source, options.Enmity);
                    break;
            }
        }
    }

    public interface ITauntComponentOptions
    {
        int Enmity { get; init; }
    }
}