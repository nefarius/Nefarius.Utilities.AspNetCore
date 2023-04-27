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

    /// <summary>
    ///     Absolute path to directory where logs will get stored.
    /// </summary>
    public string LogsDirectory { get; init; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    ///     Application (server) log file name.
    /// </summary>
    public string ServerLogFileName { get; init; } = "server-.log";

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