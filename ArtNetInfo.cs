using System.Net;
using System.Text;

namespace ArtfullySimple;

public struct ArtNetInfo
{
    public static readonly string ART_IDENTIFIER = Encoding.UTF8.GetString(new byte[8] { 65, 114, 116, 45, 78, 101, 116, 0 } );
    
    public ArtNetInfo(byte[] info)
    {
        Protocol = (ushort)IPAddress.NetworkToHostOrder((short)(info[11] << 8 | info[10]));
        IsValid = info.Length > 12 &&
                    Encoding.UTF8.GetString(info, 0, 8) == ART_IDENTIFIER &&
                    Protocol >= 14;
        
        OpCode = (ArtOp)IPAddress.NetworkToHostOrder((short)(info[8] << 8 | info[9]));
        Buffer = info;
    }

    public bool IsValid;
    public ArtOp OpCode;
    public ushort Protocol;
    public byte[] Buffer;

    public static implicit operator byte[](ArtNetInfo info)
    {
        return info.Buffer;
    }
}
