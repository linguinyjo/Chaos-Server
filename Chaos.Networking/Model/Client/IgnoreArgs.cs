using Chaos.Core.Definitions;
using Chaos.Packets.Interfaces;

namespace Chaos.Networking.Model.Client;

public record IgnoreArgs(IgnoreType IgnoreType, string? TargetName) : IReceiveArgs;