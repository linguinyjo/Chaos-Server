using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.DebuffEffects;

public class FasNadurEffect : EffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(8000);

    /// <inheritdoc />
    public override byte Icon => 90;

    /// <inheritdoc />
    public override string Name => "fas nadur";

    private static int Multiplier => 20;

    public override void OnTerminated()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { ElementalMultiplier = Multiplier });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.AddBonus(new Attributes { ElementalMultiplier = Multiplier });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
}