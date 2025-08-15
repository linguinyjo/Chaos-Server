using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Scripting.SpellScripts.WaystoneScripts;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts;

public class WaystoneSaveScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;

    /// <inheritdoc />
    public WaystoneSaveScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        Dialog = subject;
        DialogFactory = dialogFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
    }


    public override void OnDisplayed(Aisling source)
    {
        // Get waystone from args
        Subject.MenuArgs.TryGet(0, out string? waystone);
        if (waystone == null) return;

        // Parse the string to Waystone enum
        if (!Enum.TryParse<Waystone>(waystone, out var currentWaystone))
        {
            Console.WriteLine($"Invalid waystone name: {waystone}");
            return; // Or handle the error differently
        }

        if (source.Trackers.Flags.TryGetFlag<Waystone>(out var visitedWaystones))
        {
            if (visitedWaystones.HasFlag(currentWaystone))
            {
                Console.WriteLine("Player has visited this way stone!");
                var dialog = new Dialog(
                    Dialog.DialogSource,
                    DialogFactory,
                    ChaosDialogType.Normal,
                    "*On closer inspection you realise you have already commited the pattern to memory*")
                {
                    NextDialogKey = "Close"
                };
                dialog.Display(source);
            }
            else
            {
                HandleWaystoneNotVisited(source, currentWaystone);
            }
        }
        else
        {
            HandleWaystoneNotVisited(source, currentWaystone);
        }
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }

    private void HandleWaystoneNotVisited(Aisling source, Waystone currentWaystone)
    {
        Console.WriteLine("Player has NOT visited this way stone!.");
        source.Trackers.Flags.AddFlag(currentWaystone);
        Console.WriteLine("Player has visited this way stone!");
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            "*You study the pattern intently, committing each section to memory as you work your way along the intricate design*")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
        source.Client.SendSound(42, false);
    }
}