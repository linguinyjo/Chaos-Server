using Chaos.Collections;
using Chaos.Collections.Common;
using Chaos.Models.Data;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.AislingScripts;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class WithAnimationScript : DialogScriptBase
{
    private readonly ISimpleCache SimpleCache;
    
    private const byte SOUND = 29;

    private Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 50
    };

    /// <inheritdoc />
    public WithAnimationScript(Dialog subject, ISimpleCache simpleCache)
        : base(subject)
        => SimpleCache = simpleCache;

    public override void OnDisplayed(Aisling source)
    {
        source.Animate(Animation);
    }
}