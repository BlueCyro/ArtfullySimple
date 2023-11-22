using System.Buffers.Binary;

namespace ArtfullySimple;

public class ArtDmxPacket : ArtNetPacket
{
    public ArtDmxPacket(ArtNetReader reader) : base(reader)
    {
        Sequence = reader.ReadByte();
        Physical = reader.ReadByte();
        Universe = BinaryPrimitives.ReverseEndianness(reader.ReadShort()); // Universe is sent Low-High instead of High-Low, annoying.
        Length = reader.ReadShort();
        DMX = reader.ReadBytes(Length);
    }

    public byte Sequence;
    public byte Physical;
    public ushort Universe;
    public ushort Length;
    public ArraySegment<byte> DMX;
}
