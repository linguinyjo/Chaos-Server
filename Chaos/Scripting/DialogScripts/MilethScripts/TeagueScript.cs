using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts;
using Chaos.Scripting.QuestScripts.Terror;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.MilethScripts;

public class TeagueScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion
    
    /// <inheritdoc />
    public TeagueScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var playerLevel = source.UserStatSheet.Level;
        if (playerLevel is < 11 or > 41) return;
        var questStatus = TerrorQuestHelper.GetQuestStatus(source);
        switch (questStatus)
        {
            case TerrorQuestStatus.Completed:
                return;
            case TerrorQuestStatus.None or TerrorQuestStatus.GardenStarted:
                Subject.AddOption("Give 2000 coins", "teague_garden_terror_initial");
                break;
            case TerrorQuestStatus.GardenSlain:
                Subject.AddOption("I slew the terror in the garden", "teague_garden_terror_slain");
                break;
            case TerrorQuestStatus.GardenCompleted or TerrorQuestStatus.AlleyStarted:
                Subject.AddOption("Give 5000 coins", "teague_alley_terror_initial");
                break;
            case TerrorQuestStatus.AlleySlain:
                Subject.AddOption("I slew the terror in the alley", "teague_alley_terror_slain");
                break;
            case TerrorQuestStatus.AlleyCompleted or TerrorQuestStatus.CryptStarted:
                Subject.AddOption("Give 10000 coins", "teague_crypt_terror_initial");
                break;
            case TerrorQuestStatus.CryptSlain:
                Subject.AddOption("I slew the terror in the crypt", "teague_garden_terror_slain");
                break;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
