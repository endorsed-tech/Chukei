using System.Runtime.CompilerServices;

public interface IConnection
{
    public byte[] address { get; set; }
}

/// <summary>
/// A struct representing a requested connection to an IPv4 Endpoint
/// </summary>
public class IPv4Connection : IConnection
{
    public byte Size = 5;
    public byte[] address { get; set; }

    public IPv4Connection(byte[] address)
    {
        this.address = address;
    }

    /*
     * (1) Size
     * (2) ipv4Address
     * (5)
     */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSize() => 5;
}

/// <summary>
/// A struct representing a requested connection to an IPv6 Endpoint
/// </summary>
public class IPv6Connection : IConnection
{
    public byte Size = 17;
    public byte[] address { get; set; }

    public IPv6Connection(byte[] address)
    {
        this.address = address;
    }

    /*
     * (1) Size
     * (2) ipv6Address
     * (17)
     */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSize() => 17;
}

