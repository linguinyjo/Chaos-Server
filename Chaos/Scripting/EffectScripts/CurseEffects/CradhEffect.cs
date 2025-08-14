using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.CurseEffects;

public class CradhEffect : EffectBase, 
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{

    /// <inheritdoc />
    public List<string> ConflictingEffectNames { get; init; } =
        [
            "Beag Cradh",
            "Cradh",
            "Mor Cradh",
            "Not So Bad Curse"
        ];

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);
    
    /// <inheritdoc />
    public override byte Icon => 82;

    /// <inheritdoc />
    public override string Name => "Cradh";

    private const int AcDeduction = 30;

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
