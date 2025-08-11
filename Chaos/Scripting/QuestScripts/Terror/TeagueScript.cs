using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TeagueScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public TeagueScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (!TerrorQuestHelper.IsQuestAvailable(source)) return;
        
        var questStatus = TerrorQuestHelper.GetQuestStatus(source);
        if (questStatus is TerrorQuestStatus.None)
        {
            var terrorLevel = TerrorQuestHelper.GetTerrorLevel(source);
            switch (terrorLevel)
            {
                case TerrorLevel.Garden:
                    Subject.AddOption("Give 2000 coins", "teague_garden_terror_initial");
                    break;
                case TerrorLevel.Alley:
                    Subject.AddOption("Give 5000 coins", "teague_alley_terror_initial");
                    break;
                case TerrorLevel.Crypt:
                    Subject.AddOption("Give 10000 coins", "teague_crypt_terror_initial");
                    break;
                case TerrorLevel.None:
                    break;
                default: return;
            }
            return;
        }
        
        
        switch (questStatus)
        {
            case TerrorQuestStatus.GardenStarted:
                Subject.AddOption("Give 2000 coins", "teague_garden_terror_initial");
                break;
            case TerrorQuestStatus.GardenSlain:
                Subject.AddOption("I slew the terror in the garden", "teague_garden_terror_slain");
                break;
            case TerrorQuestStatus.AlleyStarted:
                Subject.AddOption("Give 5000 coins", "teague_alley_terror_initial");
                break;
            case TerrorQuestStatus.AlleySlain:
                Subject.AddOption("I slew the terror in the alley", "teague_alley_terror_slain");
                break;
            case TerrorQuestStatus.CryptStarted:
                Subject.AddOption("Give 10000 coins", "teague_crypt_terror_initial");
                break;
            case TerrorQuestStatus.CryptSlain:
                Subject.AddOption("I slew the terror in the crypt", "teague_crypt_terror_slain");
                break;
            case TerrorQuestStatus.Completed: return;
            default: return;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
