using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ArtfullySimple;

public class ArtNetClient : UdpClient
{
    public event EventHandler<ArtNetPacket>? ReceivedPacket;
    CancellationTokenSource tkSrc;
    public bool IsListening => _listening;
    private bool _listening;
    private IPEndPoint ep;

    public ArtNetClient(IPAddress ip)
    {
        ep = new(ip, 6454);
        tkSrc = new();
        Client = new Socket(ep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        Client.Bind(ep);
    }

    public void StartListening()
    {
        Task.Run(async () => await ListenLoop(tkSrc.Token));
        _listening = true;
    }

    public void StopListening()
    {
        tkSrc.Cancel();
        _listening = false;
    }

    private async Task ListenLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var result = await ReceiveAsync();
            ArtNetInfo info = new(result.Buffer);

            if (!info.IsValid)
                continue;
            
            ArtNetReader reader = new(info);

            if (reader.TryDecodePacket(out ArtNetPacket packet))
                ReceivedPacket?.Invoke(this, packet);
        }
    }
}
