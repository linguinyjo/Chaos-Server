using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class LurecaPorteForestQuestScript : DialogScriptBase
{
    private readonly Dialog Dialog;
    private readonly IDialogFactory DialogFactory;
    private readonly IItemFactory ItemFactory;

    /// <inheritdoc />
    public LurecaPorteForestQuestScript(Dialog subject, IDialogFactory dialogFactory, IItemFactory itemFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
        ItemFactory = itemFactory;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = PorteForestQuestHelper.GetQuestStatus(source);
        if (questStatus != PorteForestQuestStatus.Completed) return;

        var playerHasEasedTheSuffering = PorteForestQuestHelper.PlayerHasEasedTheSuffering(source);
        if (!playerHasEasedTheSuffering) return;
        var newDialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Menu,
            "Leave this place while the path is still open, aisling. For me it is too late..."
        );
        newDialog.Display(source);
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        // if option to give ring was selected
        if (optionIndex != 1) return;
        var hasRing = source.Inventory.HasCountByTemplateKey("tristarRing", 1);
        if (hasRing)
        {
            source.Inventory.RemoveQuantityByTemplateKey("tristarRing", 1);
            PorteForestQuestHelper.CompleteEaseTheSuffering(source);
        }
        else
            Dialog.Reply(
                source,
                "I don't see the ring you're talking about. Have I not already experienced enough cruelty? Please leave me alone...",
                "close");
    }
}