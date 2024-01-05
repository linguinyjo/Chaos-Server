using Chaos.Common.Definitions;
using Chaos.IO.Memory;
using Chaos.Networking.Definitions;
using Chaos.Networking.Entities.Server;
using Chaos.Packets.Abstractions;
using Chaos.Packets.Abstractions.Definitions;

namespace Chaos.Networking.Serializers;

/// <summary>
///     Serializes a <see cref="AddItemToPaneArgs" /> into a buffer
/// </summary>
public sealed class AddItemToPaneConverter : PacketConverterBase<AddItemToPaneArgs>
{
    /// <inheritdoc />
    public override byte OpCode => (byte)ServerOpCode.AddItemToPane;

    /// <inheritdoc />
    public override AddItemToPaneArgs Deserialize(ref SpanReader reader)
    {
        var slot = reader.ReadByte();
        var sprite = reader.ReadUInt16();
        var color = reader.ReadByte();
        var name = reader.ReadString8();
        var count = reader.ReadUInt32();
        var stackable = reader.ReadBoolean();
        var maxDurability = reader.ReadUInt32();
        var currentDurability = reader.ReadUInt32();

        return new AddItemToPaneArgs
        {
            Item = new ItemInfo
            {
                Slot = slot,
                Sprite = (ushort)(sprite - NETWORKING_CONSTANTS.ITEM_SPRITE_OFFSET),
                Color = (DisplayColor)color,
                Name = name,
                Count = count,
                Stackable = stackable,
                MaxDurability = (int)maxDurability,
                CurrentDurability = (int)currentDurability
            }
        };
    }

    /// <inheritdoc />
    public override void Serialize(ref SpanWriter writer, AddItemToPaneArgs args)
    {
        writer.WriteByte(args.Item.Slot);
        writer.WriteUInt16((ushort)(args.Item.Sprite + NETWORKING_CONSTANTS.ITEM_SPRITE_OFFSET));
        writer.WriteByte((byte)args.Item.Color);
        writer.WriteString8(args.Item.Name);
        writer.WriteUInt32(args.Item.Count!.Value);
        writer.WriteBoolean(args.Item.Stackable);
        writer.WriteUInt32((uint)args.Item.MaxDurability);
        writer.WriteUInt32((uint)args.Item.CurrentDurability);

        //nfi
        if (args.Item.Stackable)
            writer.WriteByte(0);
    }
}