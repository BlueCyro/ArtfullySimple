using System.Buffers.Binary;
using System.Net;

namespace ArtfullySimple;

public readonly struct ArtNetInfo(byte[] info)
{
    public static readonly byte[] ART_IDENTIFIER = new byte[8] { 65, 114, 116, 45, 78, 101, 116, 0 };
    public readonly bool IsValid => Buffer.Length > 12 && info.AsSpan(0, 8).SequenceEqual(ART_IDENTIFIER) && Protocol >= 14;
    public readonly ArtOp OpCode => (ArtOp)IPAddress.NetworkToHostOrder((short)(info[9] | info[8] << 8));
    public readonly ushort Protocol => (ushort)IPAddress.NetworkToHostOrder((short)(info[10] | info[11] << 8));
    public readonly byte[] Buffer => info;

    public static implicit operator byte[](ArtNetInfo info)
    {
        return info.Buffer;
    }
}
