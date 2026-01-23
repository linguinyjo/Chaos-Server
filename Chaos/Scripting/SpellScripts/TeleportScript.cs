using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.SpellScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.SpellScripts;

public class TeleportScript : ConfigurableSpellScriptBase,
    GenericAbilityComponent<Creature>.IAbilityComponentOptions,
    ShowDialogAbilityComponent.IShowDialogComponentOptions
{
    /// <inheritdoc />
    public TeleportScript(Spell subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
    }

    public int? ExclusionRange { get; init; }
    public TargetFilter Filter { get; init; }
    public bool MustHaveTargets { get; init; }
    public int Range { get; init; }
    public AoeShape Shape { get; init; }
    public bool SingleTarget { get; init; }
    public byte? Sound { get; init; }
    public ushort? AnimationSpeed { get; init; }
    public BodyAnimation BodyAnimation { get; init; }
    public bool? ScaleBodyAnimationSpeedByAttackSpeed { get; init; }
    public bool AnimatePoints { get; init; }
    public Animation? Animation { get; init; }
    public int? ManaCost { get; init; }
    public decimal PctManaCost { get; init; }
    public bool ShouldNotBreakHide { get; init; }
    public bool CanResist { get; init; }


    public IDialogFactory DialogFactory { get; init; }
    public string? DialogKey { get; init; }
    public IDialogSourceEntity? DialogSource { get; init; }

    public override void OnUse(SpellContext context)
        => new ComponentExecutor(context).WithOptions(this)
            .ExecuteAndCheck<GenericAbilityComponent<Creature>>()
            ?.Execute<ShowDialogAbilityComponent>();
}