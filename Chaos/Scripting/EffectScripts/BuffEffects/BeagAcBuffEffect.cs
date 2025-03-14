using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.Components.EffectComponents;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.BuffEffects;

public sealed class BeagAcBuffEffect : EffectBase
   
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(5);

    /// <inheritdoc />
    public override byte Icon => 94;

    /// <inheritdoc />
    public override string Name => "beag armor";
    
    private static int AcBuff => 5;

    public override void OnTerminated()
    {
        Subject.StatSheet.AddBonus(new Attributes { Ac = AcBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
    
    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.StatSheet.SubtractBonus(new Attributes { Ac = AcBuff });
        AislingSubject?.Client.SendAttributes(StatUpdateType.Full);
    }
}
