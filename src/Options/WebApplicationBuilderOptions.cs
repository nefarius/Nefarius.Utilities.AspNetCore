using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     Options to influence <see cref="WebApplicationBuilderExtensions" />.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class WebApplicationBuilderOptions
{
    internal WebApplicationBuilderOptions() { }

    public SerilogOptions Serilog { get; } = new();

    /// <summary>
    ///     W3C logging options.
    /// </summary>
    public W3CLoggingOptions W3C { get; } = new();

    /// <summary>
    ///     If set, will auto-detect local networks and add them as known networks for forwarding header options.
    /// </summary>
    /// <remarks>Only enable this if the service is run behind a reverse proxy, otherwise header spoofing is a possibility!</remarks>
    public bool AutoDetectPrivateNetworks { get; init; } = true;
}