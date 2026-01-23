using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

public sealed class OranLuth2Effect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(3);

    private static int AttackSpeedBuff => 15;
    public List<string> ConflictingEffectNames { get; init; } = ["oran luth 1", "oran luth 2", "oran luth 3"];

    /// <inheritdoc />
    public override byte Icon => 148;

    /// <inheritdoc />
    public override string Name => "oran luth 2";

    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { AtkSpeedPct = AttackSpeedBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.AddBonus(new Attributes { AtkSpeedPct = AttackSpeedBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
}