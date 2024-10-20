using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.SkillScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.SkillScripts;

public class ApplyEffectScript : ConfigurableSkillScriptBase,
                            GenericAbilityComponent<Creature>.IAbilityComponentOptions,
                            ApplyEffectAbilityComponent.IApplyEffectComponentOptions
{
    
    /// <inheritdoc />
    public ApplyEffectScript(Skill subject, IEffectFactory effectFactory)
        : base(subject)
    {
        SourceScript = this;
        EffectFactory = effectFactory;
    }

    /// <inheritdoc />
    public override void OnUse(ActivationContext context)
    {
        new ComponentExecutor(context).WithOptions(this)
            .ExecuteAndCheck<GenericAbilityComponent<Creature>>()
            ?.Execute<ApplyEffectAbilityComponent>();
    }
    
    #region ScriptVars
    /// <inheritdoc />
    public AoeShape Shape { get; init; }

    /// <inheritdoc />
    public bool SingleTarget { get; init; }

    /// <inheritdoc />
    public TargetFilter Filter { get; init; }

    /// <inheritdoc />
    public int Range { get; init; }

    /// <inheritdoc />
    public bool ExcludeSourcePoint { get; init; }

    /// <inheritdoc />
    public bool MustHaveTargets { get; init; }

    /// <inheritdoc />
    public byte? Sound { get; init; }

    /// <inheritdoc />
    public BodyAnimation BodyAnimation { get; init; }
    /// <inheritdoc />
    public ushort? AnimationSpeed { get; init; }
    /// <inheritdoc />
    public Animation? Animation { get; init; }
    /// <inheritdoc />
    public bool AnimatePoints { get; init; }
    public IScript SourceScript { get; init; }
    /// <inheritdoc />
    public int? ManaCost { get; init; }
    /// <inheritdoc />
    public decimal PctManaCost { get; init; }
    /// <inheritdoc />
    public bool ShouldNotBreakHide { get; init; }
    /// <inheritdoc />
    public bool CanResist { get; init; }
    /// <inheritdoc />
    public TimeSpan? EffectDurationOverride { get; init; }
    /// <inheritdoc />
    public IEffectFactory EffectFactory { get; init; }
    /// <inheritdoc />
    public string? EffectKey { get; init; }
    #endregion
}