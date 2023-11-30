using System.Buffers.Binary;
using System.Linq.Expressions;
using System.Net;

namespace ArtfullySimple;

public class ArtNetReader(ArtNetInfo input) : BinaryReader(new MemoryStream(input))
{
    public ushort ReadShort()
    {
        return (ushort)IPAddress.NetworkToHostOrder(ReadInt16());
    }
    
    public ushort ReadShortLittleEndian()
    {
        return BinaryPrimitives.ReverseEndianness(ReadShort());
    }

    new public ArraySegment<byte> ReadBytes(int length)
    {
        int curPos = (int)BaseStream.Position;
        ArraySegment<byte> segView = new(input, curPos, length);
        BaseStream.Position += length;
        return segView;
    }

    // If you're wondering about the weird use of '!', it's because I
    // don't have the fancy "NotNullIfTrue" attributes in net472 v.v
    public bool TryDecodePacket(out ArtNetPacket packet) 
    {
        ArtNetPacket.TryGetPacket(input.OpCode, out var newPacket);

        packet = newPacket?.Invoke(this)!;

        return packet != null;
    }
}
