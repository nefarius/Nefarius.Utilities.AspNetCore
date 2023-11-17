extern alias IPNetwork2;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using IPNetwork2::System.Net;

namespace Nefarius.Utilities.AspNetCore.Util;

internal static class NetworkUtil
{
    public static IEnumerable<IPNetwork> GetNetworks(NetworkInterfaceType type)
    {
        return from item in NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.NetworkInterfaceType == type &&
                            n.OperationalStatus ==
                            OperationalStatus.Up) // get all operational networks of a given type
                .Select(n => n.GetIPProperties()) // get the IPs
            //.Where(n => n.GatewayAddresses.Any())
            select item.UnicastAddresses.FirstOrDefault(i =>
                i.Address.AddressFamily == AddressFamily.InterNetwork)
            into ipInfo
            where ipInfo != null
            select IPNetwork.Parse(ipInfo.Address, ipInfo.IPv4Mask);
    }
}