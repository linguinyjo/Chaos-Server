using Chaos.Clients.Abstractions;
using Chaos.CommandInterceptor;
using Chaos.CommandInterceptor.Abstractions;
using Chaos.Common.Collections;
using Chaos.Common.Definitions;
using Chaos.Containers;
using Chaos.Extensions.Common;
using Chaos.Networking.Abstractions;
using Chaos.Objects.World;
using Chaos.Storage.Abstractions;

namespace Chaos.Commands;

[Command("tpto")]
public sealed class TeleportCommand : ICommand<Aisling>
{
    private readonly ISimpleCache Cache;
    private readonly IClientRegistry<IWorldClient> ClientRegistry;

    public TeleportCommand(ISimpleCache cache, IClientRegistry<IWorldClient> clientRegistry)
    {
        Cache = cache;
        ClientRegistry = clientRegistry;
    }

    /// <inheritdoc />
    public ValueTask ExecuteAsync(Aisling aisling, ArgumentCollection args)
    {
        if (!args.TryGetNext<string>(out var type))
            return default;

        switch (type.ToLower())
        {
            case "player":
                if (!args.TryGetNext<string>(out var playerName))
                    return default;

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                var player = ClientRegistry
                             .Select(c => c.Aisling)
                             .Where(a => a != null)
                             .FirstOrDefault(a => a.Name.EqualsI(playerName));

                if (player == null)
                {
                    aisling.Client.SendServerMessage(ServerMessageType.OrangeBar1, $"{playerName} is not online");

                    return default;
                }

                aisling.TraverseMap(player.MapInstance, player);

                break;
            case "map":
                if (!args.TryGetNext<string>(out var mapInstanceId))
                    return default;

                var mapInstance = Cache.Get<MapInstance>(mapInstanceId);

                Point point;

                if (args.TryGetNext<int>(out var xPos) && args.TryGetNext<int>(out var yPos))
                    point = new Point(xPos, yPos);
                else
                    point = new Point(mapInstance.Template.Width / 2, mapInstance.Template.Height / 2);

                aisling.TraverseMap(mapInstance, point);

                return default;
        }

        return default;
    }
}