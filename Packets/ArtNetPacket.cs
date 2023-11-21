namespace ArtfullySimple;

public class ArtNetPacket
{
    public ArtNetPacket(ArtNetReader reader)
    {
        reader.BaseStream.Seek(8, SeekOrigin.Begin);
        OpCode = (ArtOp)reader.ReadShort();
        Protocol = reader.ReadShort();
        PacketReader = reader;
    }
    public ArtOp OpCode;
    public ushort Protocol;
    public ArtNetReader PacketReader;
}
