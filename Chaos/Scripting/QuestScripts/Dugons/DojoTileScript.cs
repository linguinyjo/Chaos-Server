using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Extensions;
using Chaos.Extensions.Geometry;
using Chaos.Geometry.Abstractions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ReactorTileScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public class DojoTileScript : ReactorTileScriptBase
{
    private readonly ISimpleCache SimpleCache;
    private readonly IDialogFactory DialogFactory;
    private readonly IMerchantFactory MerchantFactory;

    /// <inheritdoc />
    public DojoTileScript(ReactorTile subject, ISimpleCache simpleCache, IDialogFactory dialogFactory, IMerchantFactory merchantFactory)
        : base(subject)
    {
        SimpleCache = simpleCache;
        DialogFactory = dialogFactory;
        MerchantFactory = merchantFactory;
    }
    
    /// <inheritdoc />
    public override void OnWalkedOn(Creature source)
    {
        var aisling = source as Aisling;
        var merchant = MerchantFactory.Create("kyros", source.MapInstance, source);

        var newDialog = new Dialog(
            merchant,
            DialogFactory,
            ChaosDialogType.Menu,
            "Beyond there lie the Sapphre Streams training Groves. Which one would you like to enter?")
        {
            NextDialogKey = ""
        };
        newDialog.AddScript<DojoWarpScript>();
        newDialog.Display(aisling);
    }
}