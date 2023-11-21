# ✨🔌 ArtfullySimple: Simply ArtNet

ArtfullySimple is a library for parsing [ArtNet](https://art-net.org.uk) data with no strings attached. You don't need to specify any fancy network settings, you don't need to worry about overly-complex functionality. Just create the client, subscribe, and receive packets.


# How to use
```csharp
ArtNetClient client = new(IPAddress.Any); // Or whatever IP you want to bind to

client.ReceivedPacket += (s, p) =>
{
    if (p is ArtDmxPacket packet)
    {
        ArraySegment<byte> vals = packet.DMX; // Segment of the packet that contains DMX data
        Console.WriteLine(string.Join(",", vals));
        // Example Output: 255,255,255,255,100,76,0,0 ... and so forth (your array will vary)
    }
}
client.StartListening(); // Starts listening for ArtNet asynchronously
```


### Supplementary info

For now, this library is solely for parsing incoming ArtNet DMX data packets. It currently does not support Rdm or any other part of the ArtNet standard (aside from some cursory support for reading poll packets). If you just need a dead-simple solution to receive DMX data - you've come to the right place. If you need to send ArtNet data for whatever reason then you're gonna need a little more elbow grease.

I created this library because I needed a way to easily parse ArtNet data and had issues making existing libraries work on the .NET version (4.7.2) I need. I'm also tired of these kinds of networking libraries being more complicated than they need to be. This project knows it's scope and is provided as-is.