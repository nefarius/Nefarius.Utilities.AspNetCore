using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
#if !NET8_0_OR_GREATER
// Tuple substitute, since we only care about the values
using IPNetwork = (System.Net.IPAddress BaseAddress, int PrefixLength);
#endif

namespace Nefarius.Utilities.AspNetCore.Util;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class NetworkUtil
{
    public static IEnumerable<IPNetwork> GetNetworks()
    {
        return IPGlobalProperties
            .GetIPGlobalProperties()
            .GetUnicastAddresses()
            .Select(GetIPNetwork);
    }

    /// <summary>
    ///     Computes the network prefix address from the IP and prefix length as per CIDR
    /// </summary>
    /// <param name="info">The IP info</param>
    /// <returns>The IP address representing the network prefix</returns>
    private static IPAddress GetIPNetworkPrefix(this UnicastIPAddressInformation info)
    {
        if (info.PrefixLength == 0)
        {
            return info.Address.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any;
        }

        int maxPrefix = info.Address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
        if (info.PrefixLength == maxPrefix)
        {
            return info.Address;
        }

        if (info.PrefixLength > maxPrefix)
        {
            throw new ArgumentOutOfRangeException($"{nameof(info)}.PrefixLength");
        }

        byte[] bytes = info.Address.GetAddressBytes();
        int bitsToBeZeroed = maxPrefix - info.PrefixLength;

        // Big-endian, so we zero bits right to left
        int i = bytes.Length;
        while (i-- > 0 && bitsToBeZeroed >= 8)
        {
            bytes[i] = 0;
            bitsToBeZeroed -= 8;
        }

        if (bitsToBeZeroed > 0)
        {
            bytes[i] &= (byte)(byte.MaxValue << bitsToBeZeroed);
        }

        return info.Address.AddressFamily == AddressFamily.InterNetwork
            ? new IPAddress(bytes)
            : new IPAddress(bytes, info.Address.ScopeId);
    }

    /// <summary>
    ///     Computes the IPNetwork that the IP address belongs to
    /// </summary>
    /// <param name="info">The IP info</param>
    /// <returns>The IPNetwork that th# IP belongs to</returns>
    private static IPNetwork GetIPNetwork(this UnicastIPAddressInformation info)
    {
        return new IPNetwork(info.GetIPNetworkPrefix(), info.PrefixLength);
    }
}