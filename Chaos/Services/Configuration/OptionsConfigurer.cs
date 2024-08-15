using System.Configuration;
using System.Net;
using System.Net.Sockets;
using Chaos.Common.Abstractions;
using Chaos.Common.Definitions;
using Chaos.Definitions;
using Chaos.Messaging.Abstractions;
using Chaos.MetaData;
using Chaos.Networking.Abstractions;
using Chaos.Scripting.Components.AbilityComponents;
using Chaos.Scripting.ItemScripts.Enchantments;
using Chaos.Services.Servers.Options;
using Chaos.Services.Storage.Options;
using Chaos.Utilities;
using Microsoft.Extensions.Options;

namespace Chaos.Services.Configuration;

public sealed class OptionsConfigurer(IStagingDirectory stagingDirectory, IChannelService channelService)
    : IPostConfigureOptions<IConnectionInfo>,
      IPostConfigureOptions<LobbyOptions>,
      IPostConfigureOptions<LoginOptions>,
      IPostConfigureOptions<WorldOptions>,
      IPostConfigureOptions<MetaDataStoreOptions>

{
    private readonly IChannelService ChannelService = channelService;
    private readonly IStagingDirectory StagingDirectory = stagingDirectory;

    /// <inheritdoc />
    public void PostConfigure(string? name, IConnectionInfo options)
    {
        if (!string.IsNullOrEmpty(options.HostName))
            options.Address = Dns.GetHostAddresses(options.HostName)
                                 .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)!;
    }

    /// <inheritdoc />
    public void PostConfigure(string? name, LobbyOptions options)
    {
        PostConfigure(name, (IConnectionInfo)options);

        foreach (var server in options.Servers)
            PostConfigure(name, server);
    }

    /// <inheritdoc />
    public void PostConfigure(string? name, LoginOptions options)
    {
        PostConfigure(name, (IConnectionInfo)options);
        PostConfigure(name, options.WorldRedirect);

        if (Point.TryParse(options.StartingPointStr, out var point))
            options.StartingPoint = point;
        else
            throw new ConfigurationErrorsException($"Unable to parse starting point from config ({options.StartingPointStr})");
    }

    /// <inheritdoc />
    public void PostConfigure(string? name, MetaDataStoreOptions options)
    {
        options.UseBaseDirectory(StagingDirectory.StagingDirectory);

        // ReSharper disable once ArrangeMethodOrOperatorBody
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(MagicPrefixScript.Mutate));

        //dyeable mutator
        options.PrefixMutators.Add(
            ItemMetaNodeMutator.Create(
                (node, template) =>
                {
                    if (!template.IsDyeable)
                        return [];

                    return Enum.GetNames<DisplayColor>()
                               .Select(
                                   colorName => node with
                                   {
                                       Name = $"{colorName} {node.Name}"
                                   });
                }));

        //add more mutators here
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeaponScript.Mutate));
    }

    /// <inheritdoc />
    public void PostConfigure(string? name, WorldOptions options)
    {
        PostConfigure(name, (IConnectionInfo)options);
        PostConfigure(name, options.LoginRedirect);

        foreach (var settings in options.DefaultChannels)
        {
            settings.ChannelName = ChannelService.PrependPrefix(settings.ChannelName);

            ChannelService.RegisterChannel(
                null,
                settings.ChannelName,
                settings.MessageColor ?? CHAOS_CONSTANTS.DEFAULT_CHANNEL_MESSAGE_COLOR,
                Helpers.DefaultChannelMessageHandler,
                true);
        }

        WorldOptions.Instance = options;
    }
}