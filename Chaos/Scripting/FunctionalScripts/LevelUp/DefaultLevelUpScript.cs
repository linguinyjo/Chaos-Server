using Chaos.Common.Definitions;
using Chaos.Formulae;
using Chaos.Formulae.Abstractions;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.FunctionalScripts.Abstractions;
using Chaos.Services.Servers.Options;

namespace Chaos.Scripting.FunctionalScripts.LevelUp;

public class DefaultLevelUpScript : ScriptBase, ILevelUpScript
{
    private static readonly Animation Animation = new()
    {
        AnimationSpeed = 150,
        TargetAnimation = 79
    };
    
    public ILevelUpFormula LevelUpFormula { get; set; } = LevelUpFormulae.Default;

    /// <inheritdoc />
    public static string Key { get; } = GetScriptKey(typeof(DefaultLevelUpScript));

    /// <inheritdoc />
    public static ILevelUpScript Create() => FunctionalScriptRegistry.Instance.Get<ILevelUpScript>(Key);

    /// <inheritdoc />
    public virtual void LevelUp(Aisling aisling)
    {
        aisling.Animate(Animation);
        aisling.UserStatSheet.AddLevel();
        aisling.UserStatSheet.GivePoints(2);

        if (aisling.UserStatSheet.Level < WorldOptions.Instance.MaxLevel)
        {
            var newTnl = LevelUpFormula.CalculateTnl(aisling);
            aisling.UserStatSheet.AddTnl(newTnl);
        }

        var levelUpAttribs = LevelUpFormula.CalculateAttributesIncrease(aisling);

        aisling.UserStatSheet.Add(levelUpAttribs);
        aisling.UserStatSheet.SetMaxWeight(LevelUpFormula.CalculateMaxWeight(aisling));
        aisling.UserStatSheet.SetHealthPct(100);
        aisling.UserStatSheet.SetManaPct(100);
        aisling.Client.SendAttributes(StatUpdateType.Full);
    }
}