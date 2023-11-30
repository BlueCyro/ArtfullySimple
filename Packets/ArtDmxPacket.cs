
namespace ArtfullySimple;

[ArtOpCode(ArtOp.OpDmx, typeof(ArtDmxPacket))]
public class ArtDmxPacket : ArtNetPacket
{
    public ArtDmxPacket(ArtNetReader reader) : base(reader)
    {
        Sequence = reader.ReadByte();
        Physical = reader.ReadByte();
        Universe = reader.ReadShortLittleEndian(); // Universe is sent Low-High instead of High-Low, annoying.
        Length = reader.ReadShort();
        DMX = reader.ReadBytes(Length);
    }

    public byte Sequence;
    public byte Physical;
    public ushort Universe;
    public ushort Length;
    public ArraySegment<byte> DMX;
}
