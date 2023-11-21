using System.Net;

namespace ArtfullySimple;

public class ArtNetReader(ArtNetInfo input) : BinaryReader(new MemoryStream(input))
{
    public ushort ReadShort()
    {
        return (ushort)IPAddress.NetworkToHostOrder(ReadInt16());
    }

    new public ArraySegment<byte> ReadBytes(int length)
    {
        int curPos = (int)BaseStream.Position;
        ArraySegment<byte> segView = new(input, curPos, length);
        BaseStream.Position += length;
        return segView;
    }

    public ArtNetPacket? DecodePacket()
    {        
        return input.OpCode switch
        {
            ArtOp.OpDmx => new ArtDmxPacket(this),
            ArtOp.OpPoll => new ArtPollPacket(this),
            _ => new(this),
        };
    }
}
