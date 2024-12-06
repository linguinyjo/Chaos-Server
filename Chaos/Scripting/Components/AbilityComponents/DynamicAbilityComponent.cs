using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

// Dont like this but for now it allows you to override the body animation and sound depending on what weapon is equipped
// for assail
public struct DynamicAbilityComponent<TEntity> : IConditionalComponent where TEntity: MapEntity
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
        => new ComponentExecutor(context, vars).ExecuteAndCheck<ManaCostAbilityComponent>()
                                               ?.Execute<BreaksHideAbilityComponent>()
                                               .ExecuteAndCheck<GetTargetsAbilityComponent<TEntity>>()
                                               ?.Execute<DynamicBodyAnimationAbilityComponent>()
                                               .ExecuteAndCheck<CheckResistAbilityComponent>()
                                               ?.Execute<AnimationAbilityComponent>()
                                               .Execute<DynamicSoundAbilityComponent>()
                                              
           != null;

    public interface IDynamicAbilityComponentOptions : GetTargetsAbilityComponent<TEntity>.IGetTargetsComponentOptions,
                                                DynamicSoundAbilityComponent.IDynamicSoundComponentOptions,
                                                DynamicBodyAnimationAbilityComponent.IDynamicBodyAnimationComponentOptions,
                                                AnimationAbilityComponent.IAnimationComponentOptions,
                                                ManaCostAbilityComponent.IManaCostComponentOptions,
                                                BreaksHideAbilityComponent.IBreaksHideComponentOptions,
                                                CheckResistAbilityComponent.ICheckResistComponentOptions { }
}