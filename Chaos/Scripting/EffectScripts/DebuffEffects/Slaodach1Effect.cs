using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.DebuffEffects;

public sealed class Slaodach1Effect : EffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

    /// <inheritdoc />
    public override byte Icon => 162;

    /// <inheritdoc />
    public override string Name => "slaodach 1";

    private static int AttackSpeedDeBuff => -10;

    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { AtkSpeedPct = AttackSpeedDeBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.AddBonus(new Attributes { AtkSpeedPct = AttackSpeedDeBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
}