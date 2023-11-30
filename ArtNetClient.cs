using System.Net;
using System.Net.Sockets;

namespace ArtfullySimple;

public class ArtNetClient(IPAddress ip) : UdpClient(new IPEndPoint(ip, 6454))
{
    public EventHandler<ArtNetPacket>? ReceivedPacket;
    CancellationTokenSource tkSrc = new();
    public bool IsListening => _listening;
    private bool _listening;
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
