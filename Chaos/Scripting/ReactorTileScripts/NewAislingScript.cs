using System.Diagnostics;
using Chaos.Common.Definitions;
using Chaos.Extensions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.ReactorTileScripts;

public class NewAislingScript :  ReactorTileScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public NewAislingScript(ReactorTile subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    public override void OnWalkedOn(Creature source)
    {
        if (source.Trackers.Flags.HasFlag(NewAislingFlags.HasSeenDialog)) return;
        DisplayDialog((source as Aisling)!);
        source.Trackers.Flags.AddFlag(NewAislingFlags.HasSeenDialog);
    }
    
    [Flags]
    private enum NewAislingFlags
    {
        None = 0, // Default value
        HasSeenDialog = 1 << 0, // 1
    }

    /// <inheritdoc />
    private void DisplayDialog(Aisling source)
    {
        var newDialog = new Dialog(
            source,
            DialogFactory,
            ChaosDialogType.Normal,
            "You rub your eyes...")
        {
            NextDialogKey = "new_aisling_initial"
        };
        newDialog.Display(source);
    }
}