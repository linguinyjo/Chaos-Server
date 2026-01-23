using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.CurseEffects;

public class BeagCradhEffect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

    private int AcDeduction { get; init; } = 20;

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
    [
        "Beag Cradh",
        "Cradh",
        "Mor Cradh",
        "Not So Bad Curse"
    ];

    /// <inheritdoc />
    public override byte Icon => 5;

    /// <inheritdoc />
    public override string Name => "Beag Cradh";

    public override void OnTerminated()
    {
        Subject.StatSheet.AddBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { Ac = -AcDeduction });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        var result = new ComponentExecutor(source, Subject).WithOptions(this)
            .ExecuteAndCheck<NonOverwritableEffectComponent>();
        return result != null;
    }
}