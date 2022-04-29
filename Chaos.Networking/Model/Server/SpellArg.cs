using Chaos.Core.Definitions;

namespace Chaos.Networking.Model.Server;

public record SpellArg
{
    public byte CastLines { get; set; }
    public string Name { get; set; } = null!;
    public string Prompt { get; set; } = null!;
    public byte Slot { get; set; }
    public SpellType SpellType { get; set; }
    public ushort Sprite { get; set; }
}