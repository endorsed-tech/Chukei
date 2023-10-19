using System.Net;
using System.Net.Sockets;

namespace Chukei;

public static class Server
{
    private static UdpClient? listener;
    private static readonly object listenerLock = new();

    public static async Task BeginListen(ushort port)
    {
        if (listener is not null) Program.Exit("Can not call BeginListen multiple times before calling EndListen.");
        
        lock (listenerLock)
        {
            listener = new(port);
        }

        while (true)
        {
#if DEBUG
            Console.WriteLine("Awaiting incoming connection...");
#endif
            var recResult = await listener.ReceiveAsync();

            var buffer = recResult.Buffer;

            unsafe
            {
                // If we received a buffer not sized as expected, ignore the connection
                // We add one extra byte to the size to account for packet size as first byte
                var ipv4Size = IPv4Connection.GetSize() + 1;
                var ipv6Size = IPv6Connection.GetSize() + 1;
                if (buffer.Length != ipv4Size || buffer.Length != ipv6Size) continue;

                byte size = buffer[0];

                var senderEP = recResult.RemoteEndPoint;
                var senderAddrBytes = senderEP.Address.GetAddressBytes();

                // 1 extra byte for ipv6 identifier
                var payload = new byte[senderAddrBytes.Length + 1];

                // Signal to the client if this is an IPv6 connection
                payload[0] = (byte)((senderEP.Address.AddressFamily is AddressFamily.InterNetworkV6) ? 1 : 0);
                Buffer.BlockCopy(senderAddrBytes, 0, payload, 1, senderAddrBytes.Length);

                IConnection connection;
                if (size == ipv4Size)
                {
                    connection = new IPv4Connection(buffer);

                }
                else if (size == ipv6Size)
                {
                    connection = new IPv6Connection(buffer);
                }
                else
                {
                    Program.Exit("Unreachable state found! Contact administrator immediately!");
                    return;
                }

                listener.SendAsync(payload, payload.Length, new(new IPAddress(connection.Address), port));
            }
        }
    }

    public static void EndListen()
    {
        if (listener is null) Program.Exit("Can not call EndListen before calling BeginListen.");

        lock(listenerLock)
        {
            listener.Close();
        }
    }
}
