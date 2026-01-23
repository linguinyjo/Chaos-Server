using Chaos.DarkAges.Definitions;
using Chaos.Definitions;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.EffectScripts.Abstractions;

namespace Chaos.Scripting.EffectScripts.HideEffects;

public sealed class HideEffect : EffectBase
{
    private static readonly TimeSpan TemporaryCooldown = TimeSpan.FromMilliseconds(5000);

    /// <inheritdoc />
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(2);

    /// <inheritdoc />
    public override byte Icon => 10;

    /// <inheritdoc />
    public override string Name => "Hide";

    /// <inheritdoc />
    public override void OnApplied()
    {
        Subject.SetVisibility(VisibilityType.Hidden);
        AislingSubject?.Refresh(true);
    }

    /// <inheritdoc />
    public override void OnTerminated()
    {
        Subject.SetVisibility(VisibilityType.Normal);
        if (AislingSubject == null) return;
        foreach (var spell in AislingSubject.SpellBook.Where(s => s.Template.SpellCategory == SpellCategory.Hide))
        {
            spell.BeginCooldown(AislingSubject, TemporaryCooldown);
        }

        AislingSubject?.Refresh(true);
    }

    /// <inheritdoc />
    public override bool ShouldApply(Creature source, Creature target)
    {
        if (target.Visibility is VisibilityType.Normal) return base.ShouldApply(source, target);
        AislingSubject?.SendOrangeBarMessage("You are already hidden.");
        return false;
    }
}