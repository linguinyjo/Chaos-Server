using Chaos.Collections;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts;

public class SkulledEffect(ISimpleCache simpleCache) : ContinuousAnimationEffectBase
{
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(15000);

    /// <inheritdoc />
    protected override Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 24
    };

    /// <inheritdoc />
    protected override IIntervalTimer AnimationInterval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    protected override IIntervalTimer Interval { get; } = new IntervalTimer(TimeSpan.FromMilliseconds(1500));

    /// <inheritdoc />
    public override byte Icon => 35;

    /// <inheritdoc />
    public override string Name => "Skulled";

    private const byte Sound = 6;

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        AislingSubject?.Client.SendSound(Sound, false); 
    }

    public override void OnTerminated()
    {
        if (AislingSubject == null) return; 
        AislingSubject.IsDead = true;
        var currentPosition = AislingSubject.Trackers.LastPosition;
        if (currentPosition != null)
        {
            //TODO add a chance to drop equipment
            AislingSubject.TryDrop(currentPosition, AislingSubject.Inventory, out GroundItem[] itemsToDrop);
            AislingSubject.TryDropGold(currentPosition, AislingSubject.Gold, out _);
            if (itemsToDrop != null)
                foreach (var groundItem in itemsToDrop)
                {
                    if (groundItem.Item.Template.AccountBound) continue;
                    AislingSubject.Inventory.RemoveByTemplateKey(groundItem.Item.Template.TemplateKey);
                }
        }
        
        var mapInstance = simpleCache.Get<MapInstance>("cthonicRoom2");
        var destination = new Location("cthonicRoom2",10, 10);
        AislingSubject.TraverseMap(mapInstance, destination);
        foreach (var effect in AislingSubject.Effects)
        {
            if (effect.Name != "SkulledEffect") continue;
            AislingSubject.Effects.Terminate(effect.Name);
        }
    }
    
    public override void OnDispelled() {
        if (AislingSubject != null) AislingSubject.IsDead = false;
        AislingSubject?.StatSheet.SetHp(50);
        AislingSubject?.Refresh(true);
        AislingSubject?.Display();
    }
}
