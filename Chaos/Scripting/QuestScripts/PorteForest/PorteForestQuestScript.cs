using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PorteForest;

public class PorteForestQuestScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;

    /// <inheritdoc />
    public PorteForestQuestScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = PorteForestQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case PorteForestQuestStatus.None:
            {
                PorteForestQuestHelper.StartQuest(source);
                break;
            }
            case PorteForestQuestStatus.SpokenToTorbjorn:
            {
                var hasRoots = source.Inventory.HasCountByTemplateKey("trentRoot", 3);
                if (hasRoots)
                {
                    source.Trackers.Enums.Set(PorteForestQuestStatus.DeliveredTheRoots);
                    source.Inventory.RemoveQuantityByTemplateKey("trentRoot", 3);
                    Dialog.Reply(
                        source, 
                        "Wow, you actually found the trent roots! Well a deal's a deal, I suppose.", 
                        "torbjorn_porte_information");
                }
                else
                {
                    Dialog.Reply(
                        source, 
                        "Still no trent roots, eh? Well if you do manage to find them come back to me and I'll tell you everything I know about Porte Forest.", 
                        "Close");
                }
                break;
            }
            case PorteForestQuestStatus.DeliveredTheRoots:
                Dialog.Reply(
                    source, 
                    "Ah, you're back for more information are you?", 
                    "torbjorn_porte_information");
                break;
        }
    }

    public override void OnDisplayed(Aisling source)
    {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
