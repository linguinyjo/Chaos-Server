using Chaos.Common.Definitions;
using Chaos.IO.Memory;
using Chaos.Networking.Entities.Server;
using Chaos.Packets.Abstractions;
using Chaos.Packets.Abstractions.Definitions;

namespace Chaos.Networking.Serializers;

/// <summary>
///     Serializes a <see cref="MetaDataArgs" /> into a buffer
/// </summary>
public sealed class MetaDataConverter : PacketConverterBase<MetaDataArgs>
{
    /// <inheritdoc />
    public override byte OpCode => (byte)ServerOpCode.MetaData;

    /// <inheritdoc />
    public override MetaDataArgs Deserialize(ref SpanReader reader)
    {
        var type = (MetaDataRequestType)reader.ReadByte();

        switch (type)
        {
            case MetaDataRequestType.DataByName:
            {
                var name = reader.ReadString8();
                var checkSum = reader.ReadUInt32();
                var data = reader.ReadData16();

                return new MetaDataArgs
                {
                    MetaDataRequestType = type,
                    MetaDataInfo = new MetaDataInfo
                    {
                        Name = name,
                        CheckSum = checkSum,
                        Data = data
                    }
                };
            }
            case MetaDataRequestType.AllCheckSums:
            {
                var count = reader.ReadUInt16();
                var collection = new List<MetaDataInfo>(count);

                for (var i = 0; i < count; i++)
                {
                    var name = reader.ReadString8();
                    var checkSum = reader.ReadUInt32();

                    collection.Add(
                        new MetaDataInfo
                        {
                            Name = name,
                            CheckSum = checkSum
                        });
                }

                return new MetaDataArgs
                {
                    MetaDataRequestType = type,
                    MetaDataCollection = collection
                };
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown enum value");
        }
    }

    /// <inheritdoc />
    public override void Serialize(ref SpanWriter writer, MetaDataArgs args)
    {
        writer.WriteByte((byte)args.MetaDataRequestType);

        switch (args.MetaDataRequestType)
        {
            case MetaDataRequestType.DataByName:
                writer.WriteString8(args.MetaDataInfo!.Name);
                writer.WriteUInt32(args.MetaDataInfo!.CheckSum);
                writer.WriteData16(args.MetaDataInfo!.Data);

                break;
            case MetaDataRequestType.AllCheckSums:
                writer.WriteUInt16((byte)args.MetaDataCollection!.Count);

                foreach (var info in args.MetaDataCollection!)
                {
                    writer.WriteString8(info.Name);
                    writer.WriteUInt32(info.CheckSum);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args.MetaDataRequestType), args.MetaDataRequestType, "Unknown enum value");
        }
    }
}