using Chaos.Packets.Abstractions;

namespace Chaos.Entities.Networking.Server;

public record HealthBarArgs : ISendArgs
{
    public byte HealthPercent { get; set; }
    public byte Sound { get; set; }
    public uint SourceId { get; set; }
}