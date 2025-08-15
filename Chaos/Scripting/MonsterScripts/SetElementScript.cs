using Chaos.DarkAges.Definitions;
using Chaos.Models.World;
using Chaos.Scripting.MonsterScripts.Abstractions;

namespace Chaos.Scripting.MonsterScripts;

public class SetElementScript : ConfigurableMonsterScriptBase
{
    private readonly Monster Monster;

    /// <inheritdoc />
    public SetElementScript(Monster subject)
        : base(subject)
    {
        Monster = subject;
    }

    /// <inheritdoc />
    public override void OnSpawn()
    {
        Monster.StatSheet.SetOffenseElement(ParseElement(Offense));
        Monster.StatSheet.SetDefenseElement(ParseElement(Defense));
    }

    private static Element ParseElement(string? elementName)
    {
        return Enum.TryParse<Element>(elementName, true, out var element) ? element : Element.None;
    }

    #region ScriptVars

    public string? Offense { get; init; }
    public string? Defense { get; init; }

    #endregion
}