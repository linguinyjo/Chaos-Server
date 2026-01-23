using Chaos.DarkAges.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Terror;

public class TerrorStartScript : ConfigurableDialogScriptBase
{
    /// <inheritdoc />
    public TerrorStartScript(Dialog subject) : base(subject)
    {
    }

    #region ScriptVars

    protected string? TerrorType { get; init; }

    #endregion

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var questStatus = TerrorQuestHelper.GetQuestStatus(source);

        if (!Enum.TryParse<TerrorLevel>(TerrorType, true, out var terrorLevel) || terrorLevel == TerrorLevel.None)
            return; // Invalid terror type

        // Determine gold cost based on terror level
        var goldCost = terrorLevel switch
        {
            TerrorLevel.Garden => 2000,
            TerrorLevel.Alley => 5000,
            TerrorLevel.Crypt => 10000,
            _ => 0
        };

        if (goldCost == 0) return; // Invalid terror level
        var goldTaken = source.TryTakeGold(goldCost);
        if (goldTaken == false) return;

        var expectedStartedStatus = terrorLevel switch
        {
            TerrorLevel.Garden => TerrorQuestStatus.GardenStarted,
            TerrorLevel.Alley => TerrorQuestStatus.AlleyStarted,
            TerrorLevel.Crypt => TerrorQuestStatus.CryptStarted,
            _ => TerrorQuestStatus.None
        };

        if (questStatus == expectedStartedStatus) return;

        TerrorQuestHelper.StartQuest(source, terrorLevel);
    }

    public override void OnDisplayed(Aisling source)
    {
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
    }
}