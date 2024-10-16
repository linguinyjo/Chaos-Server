using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct RemoveEffectAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IRemoveEffectComponentOptions>();

        if (string.IsNullOrEmpty(options.EffectKey))
            return;

        var targets = vars.GetTargets<Creature>();
        var effectKeys = options.EffectKey.Split(',').Select(e => e.Trim()).ToList();

        foreach (var target in targets)
        {
            // if (!target.Effects.Contains(options.EffectKey)) continue;
            // var effect = options.EffectFactory.Create(options.EffectKey);
            // target.Effects.Dispel(effect.Name);
            foreach (var effect in from effectKey in effectKeys 
                     where target.Effects.Contains(effectKey) 
                     select options.EffectFactory.Create(effectKey))
            {
                target.Effects.Dispel(effect.Name);
                // break; // Exit the inner loop after removing one effect
            }
        }
    }

    public interface IRemoveEffectComponentOptions
    {
        IEffectFactory EffectFactory { get; init; }
        string? EffectKey { get; init; }
    }
}