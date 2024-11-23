using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;

namespace Chaos.Scripting.DialogScripts.Tagor;

public class ScribeScript:  DialogScriptBase
{
    private readonly IWarriorMembershipHelper _warriorMembershipHelper;

    private static class MembershipPrices
    {
        public const int OneDay = 50000;
        public const int OneWeek = 100000;
        public const int OneMonth = 150000;
    }

    private static class MembershipDurations
    {
        public static readonly TimeSpan OneDay = TimeSpan.FromHours(24);
        public static readonly TimeSpan OneWeek = TimeSpan.FromDays(7);
        public static readonly TimeSpan OneMonth = TimeSpan.FromDays(30);
    }

    /// <inheritdoc />
    public ScribeScript(Dialog subject, IWarriorMembershipHelper? membershipHelper = null) : base(subject)
    {
        _warriorMembershipHelper = membershipHelper ?? new WarriorMembershipHelper();

    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var trackerEvent = _warriorMembershipHelper.MembershipIsActive(source);
        if (trackerEvent is not null)
        {
            Subject.Reply(source, $"You still have {trackerEvent.Remaining.Days} days and {trackerEvent.Remaining.Hours} hours left on your membership here.", "close");
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        if (!optionIndex.HasValue) return;

        switch (optionIndex.Value)
        {
            case 0: // Do nothing for option 0
                break;
            case 1:
                ProcessMembership(source, MembershipDurations.OneDay, MembershipPrices.OneDay);
                break;
            case 2:
                ProcessMembership(source, MembershipDurations.OneWeek, MembershipPrices.OneWeek);
                break;
            case 3:
                ProcessMembership(source, MembershipDurations.OneMonth, MembershipPrices.OneMonth);
                break;
            case 4: // Do nothing for option 4
                break;
        }
    }
    
    private void ProcessMembership(Aisling source, TimeSpan duration, int price)
    {
        var success = _warriorMembershipHelper.BeginMembership(source, duration, price);
        if (success)
            MembershipStartedDialog(source);
        else
            NotEnoughGoldDialog(source);
    }

    private void NotEnoughGoldDialog(Aisling source)
    {
        Subject.Reply(source, $"You don't have enough gold to do that.", "close");
    }

    private void MembershipStartedDialog(Aisling source)
    {
        Subject.Reply(source, "Excellent choice. Your memembership begins now. Please enjoy all that we have to offer here", "close");
    }
}
