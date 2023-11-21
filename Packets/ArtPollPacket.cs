namespace ArtfullySimple;

public class ArtPollPacket : ArtNetPacket
{
    public ArtPollPacket(ArtNetReader reader) : base(reader)
    {
        Flags = reader.ReadByte();
        Priority = reader.ReadByte();
    }
    public byte Flags;
    public byte Priority;
}