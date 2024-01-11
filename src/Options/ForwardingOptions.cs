using System;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     Options to influence HTTP header forwarding behaviour.
/// </summary>
public sealed class ForwardingOptions
{
    private bool _autoDetectPrivateNetworks = true;

    private bool _allowFromAny = false;

    internal ForwardingOptions() { }

    /// <summary>
    ///     If set, will auto-detect local networks and add them as known networks for forwarding header options.
    /// </summary>
    /// <exception cref="ArgumentException">This can not be enabled if <see cref="AllowFromAny"/> is enabled.</exception>
    /// <remarks>Only enable this if the service is run behind a reverse proxy, otherwise header spoofing is a possibility!</remarks>
    /// <example>This is useful if the service is running inside a local Docker environment and behind a reverse proxy.</example>
    public bool AutoDetectPrivateNetworks
    {
        get => _autoDetectPrivateNetworks;
        set
        {
            if (value && _allowFromAny)
            {
                throw new ArgumentException(
                    $"{nameof(AutoDetectPrivateNetworks)} can't be enabled if {nameof(AllowFromAny)} is enabled");
            }

            _autoDetectPrivateNetworks = value;
        }
    }

    /// <summary>
    ///     If set, all remote addresses will be treated as a proxy and therefore Forwarded For headers will be parsed.
    /// </summary>
    /// <exception cref="ArgumentException">This can not be enabled if <see cref="AutoDetectPrivateNetworks"/> is enabled.</exception>
    /// <remarks>Only enable this if the service is run behind a reverse proxy, otherwise header spoofing is a possibility!</remarks>
    /// <example>This is useful if the service is running within a Kubernetes cluster with (multiple) ingress controllers.</example>
    public bool AllowFromAny
    {
        get => _allowFromAny;
        set
        {
            if (value && _autoDetectPrivateNetworks)
            {
                throw new ArgumentException(
                    $"{nameof(AllowFromAny)} can't be enabled if {nameof(AutoDetectPrivateNetworks)} is enabled");
            }

            _allowFromAny = value;
        }
    }
}