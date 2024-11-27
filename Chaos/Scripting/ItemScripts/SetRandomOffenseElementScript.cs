using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.ItemScripts.Abstractions;
using Chaos.Scripting.ItemScripts.Enchantments.EarthScripts;
using Chaos.Scripting.ItemScripts.Enchantments.FireScripts;
using Chaos.Scripting.ItemScripts.Enchantments.SeaScripts;
using Chaos.Scripting.ItemScripts.Enchantments.WindScripts;

namespace Chaos.Scripting.ItemScripts;

// There is probably a much better way of handling assigning a random element to an item on drop which doesnt involve
// applying an item script from inside the enchant script
public class SetRandomOffenseElementScript : ItemScriptBase
{
    /// <inheritdoc />
    public SetRandomOffenseElementScript(Item subject)
        : base(subject) { }

    public override void OnDropped(Creature source, MapInstance mapInstance)
    {
        base.OnDropped(source, mapInstance);
        if (source is Aisling) return;
        Element[] allowedElements = [Element.Water, Element.Wind, Element.Earth, Element.Fire];
        var element = allowedElements.PickRandomWeightedSingle(1);
        switch (element)
        {
            case Element.Fire:
                Subject.AddScript<FireOffensePrefixScript>();
                break;
            case Element.Water:
                Subject.AddScript<SeaOffensePrefixScript>();
                break;
            case Element.Wind:
                Subject.AddScript<WindOffensePrefixScript>();
                break;
            case Element.Earth:
                Subject.AddScript<EarthOffensePrefixScript>();
                break;
        }
    }
}
