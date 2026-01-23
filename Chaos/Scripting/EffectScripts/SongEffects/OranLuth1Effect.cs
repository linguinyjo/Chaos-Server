using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.SongEffects;

public sealed class OranLuth1Effect : EffectBase,
    NonOverwritableEffectComponent.INonOverwritableEffectComponentOptions
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

    private static int AttackSpeedBuff => 10;
    public List<string> ConflictingEffectNames { get; init; } = ["oran luth 1", "oran luth 2", "oran luth 3"];

    /// <inheritdoc />
    public override byte Icon => 148;

    /// <inheritdoc />
    public override string Name => "oran luth 1";

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