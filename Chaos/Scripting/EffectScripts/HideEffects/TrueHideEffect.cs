using Chaos.Definitions;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.HideEffects;

public sealed class TrueHideEffect : EffectBase
{
    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

    /// <inheritdoc />
    public override byte Icon => 10;

    /// <inheritdoc />
    public override string Name => "True Hide";

    /// <inheritdoc />
    public override void OnApplied() => Subject.SetVisibility(VisibilityType.TrueHidden);

    /// <inheritdoc />
    public override void OnTerminated() => Subject.SetVisibility(VisibilityType.Normal);

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        if (target.Visibility is not VisibilityType.Normal)
        {
            AislingSubject?.SendOrangeBarMessage("You are already hidden.");

            return false;
        }

        return base.ShouldApply(source, target);
    }
}