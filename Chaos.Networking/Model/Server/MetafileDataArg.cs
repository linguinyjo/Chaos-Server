namespace Chaos.Networking.Model.Server;

public record MetafileDataArg
{
    public uint CheckSum { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string Name { get; set; } = null!;
}