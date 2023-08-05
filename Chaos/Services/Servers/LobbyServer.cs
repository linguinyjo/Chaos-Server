using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Chaos.Common.Abstractions;
using Chaos.Common.Definitions;
using Chaos.Common.Identity;
using Chaos.Extensions;
using Chaos.Networking.Abstractions;
using Chaos.Networking.Entities;
using Chaos.Networking.Entities.Client;
using Chaos.Packets;
using Chaos.Packets.Abstractions;
using Chaos.Packets.Abstractions.Definitions;
using Chaos.Services.Servers.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaos.Services.Servers;

public sealed class LobbyServer : ServerBase<ILobbyClient>, ILobbyServer<ILobbyClient>
{
    private readonly IFactory<ILobbyClient> ClientFactory;
    private readonly ServerTable ServerTable;
    private new LobbyOptions Options { get; }

    public LobbyServer(
        IClientRegistry<ILobbyClient> clientRegistry,
        IFactory<ILobbyClient> clientFactory,
        IRedirectManager redirectManager,
        IPacketSerializer packetSerializer,
        IOptions<LobbyOptions> options,
        ILogger<LobbyServer> logger
    )
        : base(
            redirectManager,
            packetSerializer,
            clientRegistry,
            options,
            logger)
    {
        Options = options.Value;
        ClientFactory = clientFactory;
        ServerTable = new ServerTable(Options.Servers);

        IndexHandlers();
    }

    #region OnHandlers
    public ValueTask OnVersion(ILobbyClient client, in ClientPacket packet)
    {
        var args = PacketSerializer.Deserialize<VersionArgs>(in packet);

        ValueTask InnerOnVersion(ILobbyClient localClient, VersionArgs localArgs)
        {
            if (localArgs.Version != 741)
                return default;

            localClient.SendConnectionInfo(ServerTable.CheckSum);

            return default;
        }

        return ExecuteHandler(client, args, InnerOnVersion);
    }

    public ValueTask OnServerTableRequest(ILobbyClient client, in ClientPacket packet)
    {
        var args = PacketSerializer.Deserialize<ServerTableRequestArgs>(in packet);

        ValueTask InnerOnServerTableRequest(ILobbyClient localClient, ServerTableRequestArgs localArgs)
        {
            (var serverTableRequestType, var serverId) = localArgs;

            switch (serverTableRequestType)
            {
                case ServerTableRequestType.ServerId:
                    if (ServerTable.Servers.TryGetValue(serverId!.Value, out var serverInfo))
                    {
                        var redirect = new Redirect(
                            EphemeralRandomIdGenerator<uint>.Shared.NextId,
                            serverInfo,
                            ServerType.Login,
                            client.Crypto.Key,
                            client.Crypto.Seed);

                        RedirectManager.Add(redirect);

                        Logger.LogDebug(
                            "Redirecting {@ClientIp} to {@ServerIp}",
                            client.RemoteIp,
                            serverInfo.Address.ToString());

                        client.SendRedirect(redirect);
                    } else
                        throw new InvalidOperationException($"Server id \"{serverId}\" requested, but does not exist.");

                    break;
                case ServerTableRequestType.RequestTable:
                    client.SendServerTable(ServerTable);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return default;
        }

        return ExecuteHandler(client, args, InnerOnServerTableRequest);
    }
    #endregion

    #region Connection / Handler
    public override ValueTask HandlePacketAsync(ILobbyClient client, in ClientPacket packet)
    {
        var opCode = packet.OpCode;
        var handler = ClientHandlers[(byte)opCode];

        if (handler is not null)
            Logger.WithProperty(client)
                  .LogTrace("Processing message with code {@OpCode} from {@ClientIp}", opCode, client.RemoteIp);
        else
            Logger.WithProperty(client)
                  .WithProperty(packet.ToString(), "HexData")
                  .LogWarning("Unknown message with code {@OpCode} from {@ClientIp}", opCode, client.RemoteIp);

        return handler?.Invoke(client, in packet) ?? default;
    }

    protected override void IndexHandlers()
    {
        if (ClientHandlers == null!)
            return;

        base.IndexHandlers();

        ClientHandlers[(byte)ClientOpCode.Version] = OnVersion;
        ClientHandlers[(byte)ClientOpCode.ServerTableRequest] = OnServerTableRequest;
    }

    protected override void OnConnection(IAsyncResult ar)
    {
        var serverSocket = (Socket)ar.AsyncState!;
        var clientSocket = serverSocket.EndAccept(ar);

        serverSocket.BeginAccept(OnConnection, serverSocket);

        var ip = clientSocket.RemoteEndPoint as IPEndPoint;
        Logger.LogDebug("Incoming connection from {@ClientIp}", ip!.Address);

        var client = ClientFactory.Create(clientSocket);

        Logger.WithProperty(client)
              .LogInformation("Connection established with {@ClientIp}", client.RemoteIp);

        if (!ClientRegistry.TryAdd(client))
        {
            var stackTrace = new StackTrace(true).ToString();

            Logger.WithProperty(client.Id)
                  .WithProperty(stackTrace)
                  .LogError("Somehow, two clients got the same id");

            client.Disconnect();

            return;
        }

        client.OnDisconnected += OnDisconnect;

        client.BeginReceive();
        client.SendAcceptConnection();
    }

    private void OnDisconnect(object? sender, EventArgs e)
    {
        var client = (ILobbyClient)sender!;
        ClientRegistry.TryRemove(client.Id, out _);
    }
    #endregion
}