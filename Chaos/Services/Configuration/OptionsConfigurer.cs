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
using Chaos.Scripting.ItemScripts.Enchantments.EarthScripts;
using Chaos.Scripting.ItemScripts.Enchantments.FireScripts;
using Chaos.Scripting.ItemScripts.Enchantments.SeaScripts;
using Chaos.Scripting.ItemScripts.Enchantments.WindScripts;
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

        // Weapon enchant mutators
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon1Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon2Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon3Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon4Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon5Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon6Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon7Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon8Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon9Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon10Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon11Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon12Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon13Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon14Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon15Script.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EnchantWeapon16Script.Mutate));
        
        // Element enchant mutators
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EarthDefensePrefixScript.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(EarthOffensePrefixScript.Mutate));
        
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(FireDefensePrefixScript.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(FireOffensePrefixScript.Mutate));

        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(SeaOffensePrefixScript.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(SeaDefensePrefixScript.Mutate));

        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(WindOffensePrefixScript.Mutate));
        options.PrefixMutators.Add(ItemMetaNodeMutator.Create(WindDefensePrefixScript.Mutate));
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