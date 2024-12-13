using Chaos.Collections.Time;
using Chaos.Models.World;

namespace Chaos.Scripting.DialogScripts.Tagor;

public interface IWarriorMembershipHelper
{
    TimedEventCollection.Event? MembershipIsActive(Aisling source);
    bool BeginMembership(Aisling source, TimeSpan duration, int price);
}

public class WarriorMembershipHelper : IWarriorMembershipHelper
{
    private const string WarriorMembershipTracker = "WarriorMembershipTracker";

    public TimedEventCollection.Event? MembershipIsActive(Aisling source)
    {
        source.Trackers.TimedEvents.HasActiveEvent(WarriorMembershipTracker, out var trackerEvent);
        return trackerEvent;
    }

    public bool BeginMembership(Aisling source, TimeSpan duration, int price)
    {
        if (!source.TryTakeGold(price))
            return false;
            
        source.Trackers.TimedEvents.AddEvent(WarriorMembershipTracker, duration, true);
        return true;
    }
}