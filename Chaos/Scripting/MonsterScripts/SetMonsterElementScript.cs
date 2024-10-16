using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Models.World;
using Chaos.Scripting.MonsterScripts.Abstractions;

namespace Chaos.Scripting.MonsterScripts;

public class SetMonsterElementScript : MonsterScriptBase
{
    private readonly Monster Monster;

    /// <inheritdoc />
    public SetMonsterElementScript(Monster subject)
        : base(subject)
    {
        Monster = subject;
    }

    /// <inheritdoc />
    public override void OnSpawn()
    {
        Element[] allowedElements = [Element.Water, Element.Wind, Element.Earth, Element.Fire];
        
        var offensiveElement = allowedElements.PickRandomWeightedSingle(1);
        var defensiveElement = allowedElements.PickRandomWeightedSingle(1);

        Monster.StatSheet.SetOffenseElement(offensiveElement);
        Monster.StatSheet.SetDefenseElement(defensiveElement);
        Console.WriteLine(Monster);
    } 
}
