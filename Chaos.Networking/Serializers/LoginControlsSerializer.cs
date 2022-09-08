using Chaos.Entities.Networking.Server;
using Chaos.IO.Memory;
using Chaos.Packets.Abstractions;

namespace Chaos.Networking.Serializers;

public record LoginControlsSerializer : ServerPacketSerializer<LoginControlArgs>
{
    public override ServerOpCode ServerOpCode => ServerOpCode.LoginControls;

    public override void Serialize(ref SpanWriter writer, LoginControlArgs args)
    {
        writer.WriteByte((byte)args.LoginControlsType);
        writer.WriteString8(args.Message);
    }
}