using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

#if !NET8_0_OR_GREATER
// Tuple substitute, since we only care about the values
using IPNetwork = (System.Net.IPAddress BaseAddress, int PrefixLength);
#endif

namespace Nefarius.Utilities.AspNetCore.Util;

internal static class NetworkUtil
{
    public static IEnumerable<IPNetwork> GetNetworks()
        => IPGlobalProperties
            .GetIPGlobalProperties()
            .GetUnicastAddresses()
            .Select(GetIPNetwork);

    /// <summary>
    /// Computes the network prefix address from the IP and prefix length as per CIDR
    /// </summary>
    /// <param name="info">The IP info</param>
    /// <returns>The IP address representing the network prefix</returns>
    public static IPAddress GetIPNetworkPrefix(this UnicastIPAddressInformation info)
    {
        if (info.PrefixLength == 0)
            return info.Address.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any;
        var maxPrefix = info.Address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
        if (info.PrefixLength == maxPrefix)
            return info.Address;
        if (info.PrefixLength > maxPrefix)
            throw new ArgumentOutOfRangeException("info.PrefixLength");

        var bytes = info.Address.GetAddressBytes();
        var bitsToBeZeroed = maxPrefix - info.PrefixLength;

        // Big-endian, so we zero bits right to left
        var i = bytes.Length;
        while (i --> 0 && bitsToBeZeroed >= 8)
        {
            bytes[i] = 0;
            bitsToBeZeroed -= 8;
        }
        if (bitsToBeZeroed > 0)
        {
            bytes[i] &= (byte)(byte.MaxValue << bitsToBeZeroed);
        }
        return new IPAddress(bytes, info.Address.ScopeId);
    }

    /// <summary>
    /// Computes the IPNetwork that the IP address belongs to
    /// </summary>
    /// <param name="info">The IP info</param>
    /// <returns>The IPNetwork that th# IP belongs to</returns>
    public static IPNetwork GetIPNetwork(this UnicastIPAddressInformation info)
        => new(info.GetIPNetworkPrefix(), info.PrefixLength);
}