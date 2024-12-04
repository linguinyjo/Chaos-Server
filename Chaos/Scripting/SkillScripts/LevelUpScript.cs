using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.ApplyDamage;
using Chaos.Scripting.SkillScripts.Abstractions;

namespace Chaos.Scripting.SkillScripts;

// public class LevelUpScript : ConfigurableSkillScriptBase,
//                             GenericAbilityComponent<Creature>.IAbilityComponentOptions,
//                             AbilityLevellingAbilityComponent.IAbilityLevellingComponentOptions
// {
//     /// <inheritdoc />
//     public LevelUpScript(Skill subject)
//         : base(subject)
//     {
//         ApplyDamageScript = ApplyAttackDamageScript.Create();
//         SourceScript = this;
//         AbilityTemplateKey = subject.Template.TemplateKey;
//         IsSpell = false;
//     }
//
//     /// <inheritdoc />
//     public override void OnUse(ActivationContext context)
//     {
//         //TODO maybe this should be moved to the Genericability component because leveling the ability is coupled to
//         //   whether the ability gets used/cast and its hard to know that from outside the script thats executes
//         //   that component
//         new ComponentExecutor(context).WithOptions(this)
//             .ExecuteAndCheck<GenericAbilityComponent<Creature>>()
//             ?.Execute<AbilityLevellingAbilityComponent>();
//     }
//     
//     #region ScriptVars
//     /// <inheritdoc />
//     public AoeShape Shape { get; init; }
//
//     /// <inheritdoc />
//     public bool SingleTarget { get; init; }
//
//     /// <inheritdoc />
//     public TargetFilter Filter { get; init; }
//
//     /// <inheritdoc />
//     public int Range { get; init; }
//
//     /// <inheritdoc />
//     public bool ExcludeSourcePoint { get; init; }
//
//     /// <inheritdoc />
//     public bool MustHaveTargets { get; init; }
//
//     /// <inheritdoc />
//     public byte? Sound { get; init; }
//
//     /// <inheritdoc />
//     public BodyAnimation BodyAnimation { get; init; }
//
//     /// <inheritdoc />
//     public ushort? AnimationSpeed { get; init; }
//
//     /// <inheritdoc />
//     public Animation? Animation { get; init; }
//
//     /// <inheritdoc />
//     public bool AnimatePoints { get; init; }
//
//     public IApplyDamageScript ApplyDamageScript { get; init; }
//
//     public IScript SourceScript { get; init; }
//
//     /// <inheritdoc />
//     public int? ManaCost { get; init; }
//
//     /// <inheritdoc />
//     public decimal PctManaCost { get; init; }
//
//     /// <inheritdoc />
//     public bool ShouldNotBreakHide { get; init; }
//
//     /// <inheritdoc />
//     public bool CanResist { get; init; }
//     
//     /// <inheritdoc />
//     public AbilityLevellingRate Rate { get; init; }
//
//     public string AbilityTemplateKey { get; init; }
//     public bool IsSpell { get; init; }
//     #endregion
// }

