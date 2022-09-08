using Chaos.Common.Definitions;
using Chaos.Packets.Abstractions;

namespace Chaos.Entities.Networking.Server;

public record EffectArgs : ISendArgs
{
    public EffectColor EffectColor { get; set; }
    public byte EffectIcon { get; set; }
}