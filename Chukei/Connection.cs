using System.Runtime.CompilerServices;

namespace Chukei;

public interface IConnection
{
    public byte[] Address { get; set; }
}

/// <summary>
/// A struct representing a requested connection to an IPv4 Endpoint
/// </summary>
public class IPv4Connection : IConnection
{
    public byte[] Address { get; set; }

    public IPv4Connection(byte[] address) =>
        Address = address;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetSize() => 4;
}

/// <summary>
/// A struct representing a requested connection to an IPv6 Endpoint
/// </summary>
public class IPv6Connection : IConnection
{
    public byte[] Address { get; set; }

    public IPv6Connection(byte[] address) =>
        Address = address;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSize() => 16;

}
