using Chaos.Collections;
using Chaos.DarkAges.Definitions;
using Chaos.DarkAges.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.EffectScripts.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.Time;
using Chaos.Time.Abstractions;

namespace Chaos.Scripting.EffectScripts;

public class SkulledEffect(ISimpleCache simpleCache) : ContinuousAnimationEffectBase
{
    private const int ItemLockTime = 86400;

    private const byte Sound = 6;
    protected override TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(20000);

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
    public override byte Icon => 89;

    /// <inheritdoc />
    public override string Name => "Skulled";

    /// <inheritdoc />
    protected override void OnIntervalElapsed()
    {
        AislingSubject?.Client.SendSound(Sound, false);
    }

    public override void OnTerminated()
    {
        if (AislingSubject == null) return;
        AislingSubject.IsDead = true;
        AislingSubject.TryDropAllEquipment();
        var currentPosition = AislingSubject.GetCurrentLocation();
        var droppedInventory = AislingSubject.TryDrop(currentPosition, AislingSubject.Inventory, out var itemsToDrop);
        if (droppedInventory && itemsToDrop != null)
        {
            foreach (var groundItem in itemsToDrop)
            {
                groundItem.LockToAislings(ItemLockTime, AislingSubject);
            }
        }

        var droppedMoney = AislingSubject.TryDropGold(currentPosition, AislingSubject.Gold, out var money);
        if (droppedMoney && money != null)
        {
            money.LockToAislings(ItemLockTime, AislingSubject);
        }

        if (itemsToDrop != null)
            foreach (var groundItem in itemsToDrop)
            {
                groundItem.LockToAislings(ItemLockTime, AislingSubject);
                AislingSubject.Inventory.RemoveByTemplateKey(groundItem.Item.Template.TemplateKey);
            }

        var mapInstance = simpleCache.Get<MapInstance>("cthonicRoom2");
        var destination = new Location("cthonicRoom2", 10, 10);
        AislingSubject.TraverseMap(mapInstance, destination);
        foreach (var effect in AislingSubject.Effects)
        {
            if (effect.Name == "SkulledEffect") continue;
            AislingSubject.Effects.Terminate(effect.Name);
        }
    }

    public override void OnDispelled()
    {
        if (AislingSubject != null) AislingSubject.IsDead = false;
        AislingSubject?.StatSheet.SetHp(50);
        AislingSubject?.Client.SendAttributes(StatUpdateType.Vitality);
    }
}