using Chaos.Collections.Common;
using Chaos.Messaging.Abstractions;
using Chaos.Models.World;

namespace Chaos.Messaging.Admin;

[Command("destroyall", helpText: "<gold|coins|money|grounditems|items|monsters|merchants>")]
public class DestroyAllCommand : ICommand<Aisling>
{
    /// <inheritdoc />
    public ValueTask ExecuteAsync(Aisling source, ArgumentCollection args)
    {
        var map = source.MapInstance;

        if (!args.TryGetNext<string>(out var clearType))
            return default;

        switch (clearType.ToLower())
        {
            case "gold":
            case "coins":
            case "money":
                foreach (var money in map.GetEntities<Money>()
                                         .ToList())
                    map.RemoveObject(money);

                break;
            case "grounditems":
            case "items":
                foreach (var groundItem in map.GetEntities<GroundItem>()
                                              .ToList())
                    map.RemoveObject(groundItem);

                break;
            case "monsters":
                foreach (var monster in map.GetEntities<Monster>()
                                           .ToList())
                    map.RemoveObject(monster);

                break;
            case "merchants":
                foreach (var merchant in map.GetEntities<Merchant>()
                                            .ToList())
                    map.RemoveObject(merchant);

                break;
        }

        return default;
    }
}