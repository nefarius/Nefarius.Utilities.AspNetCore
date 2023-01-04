using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using Microsoft.AspNetCore.HttpOverrides;

namespace Nefarius.Utilities.AspNetCore.Util;

internal static class NetworkUtil
{
    public static IEnumerable<IPNetwork> GetNetworks(NetworkInterfaceType type)
    {
        foreach (IPInterfaceProperties item in NetworkInterface.GetAllNetworkInterfaces()
                     .Where(n => n.NetworkInterfaceType == type &&
                                 n.OperationalStatus ==
                                 OperationalStatus.Up) // get all operational networks of a given type
                     .Select(n => n.GetIPProperties()) // get the IPs
                     .Where(n => n.GatewayAddresses.Any())) // where the IPs have a gateway defined
        {
            UnicastIPAddressInformation ipInfo =
                item.UnicastAddresses.FirstOrDefault(i =>
                    i.Address.AddressFamily == AddressFamily.InterNetwork); // get the first cluster-facing IP address
            if (ipInfo == null)
            {
                continue;
            }

            // convert the mask to bits
            byte[] maskBytes = ipInfo.IPv4Mask.GetAddressBytes();
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(maskBytes);
            }

            BitArray maskBits = new(maskBytes);

            // count the number of "true" bits to get the CIDR mask
            int cidrMask = maskBits.Cast<bool>().Count(b => b);

            // convert my application's ip address to bits
            byte[] ipBytes = ipInfo.Address.GetAddressBytes();
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(maskBytes);
            }

            BitArray ipBits = new(ipBytes);

            // and the bits with the mask to get the start of the range
            BitArray maskedBits = ipBits.And(maskBits);

            // Convert the masked IP back into an IP address
            byte[] maskedIpBytes = new byte[4];
            maskedBits.CopyTo(maskedIpBytes, 0);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(maskedIpBytes);
            }

            IPAddress rangeStartIp = new(maskedIpBytes);

            // return the start IP and CIDR mask
            yield return new IPNetwork(rangeStartIp, cidrMask);
        }
    }
}