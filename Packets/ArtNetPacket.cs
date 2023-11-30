using System.Linq.Expressions;
using System.Reflection;

namespace ArtfullySimple;


public abstract class ArtNetPacket
{
    private static readonly Dictionary<ArtOp, Func<ArtNetReader, ArtNetPacket>> _packetMap;

    static ArtNetPacket()
    {
        Type packetType = typeof(ArtNetPacket);

        _packetMap = // Find all packets with the opcode attribute and make an efficient lookup to instantiate different types of packets dynamically
            packetType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(packetType) && !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<ArtOpCodeAttribute>() != null)
                .ToDictionary(
                t =>
                    t.GetCustomAttribute<ArtOpCodeAttribute>().Op,
                t =>
                {
                    var constInfo = t.GetConstructor(new[] { typeof(ArtNetReader) });
                    var param = Expression.Parameter(typeof(ArtNetReader), "reader");
                    var exp = Expression.New(constInfo, param);
                    var lambda = Expression.Lambda<Func<ArtNetReader, ArtNetPacket>>(exp, param);
                    
                    return lambda.Compile();
                });
    }
    
    public static bool TryGetPacket(ArtOp opcode, out Func<ArtNetReader, ArtNetPacket> newPacket)
    {
        return _packetMap.TryGetValue(opcode, out newPacket);
    }

    public ArtNetPacket(ArtNetReader reader)
    {
        reader.BaseStream.Seek(8, SeekOrigin.Begin);
        OpCode = (ArtOp)reader.ReadShortLittleEndian(); // OpCode is sent Low-High instead of High-Low, ArtNet is wacky >:(
        Protocol = reader.ReadShort();
        PacketReader = reader;
    }
    public ArtOp OpCode;
    public ushort Protocol;
    public ArtNetReader PacketReader;
}
