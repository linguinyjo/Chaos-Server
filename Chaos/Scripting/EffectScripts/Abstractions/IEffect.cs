using Chaos.Common.Definitions;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts.Abstractions;

public partial interface IEffect : IDeltaUpdatable, IScript
{
    EffectColor Color { get; set; }
    TimeSpan Remaining { get; set; }
    Creature Subject { get; set; }
    Aisling? AislingSubject { get; }
    byte Icon { get; }
    string Name { get; }
    void OnApplied();
    void OnDispelled();
    void OnReApplied();
    void OnTerminated();
    void SetDuration(TimeSpan duration);
    bool ShouldApply(Creature source, Creature target);
}