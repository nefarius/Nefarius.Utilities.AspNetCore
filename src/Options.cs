using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.AspNetCore.HttpLogging;

namespace Nefarius.Utilities.AspNetCore;

/// <summary>
///     W3C Logging options.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class W3CLoggingOptions
{
    internal W3CLoggingOptions() { }

    /// <summary>
    ///     Fields to include in log.
    /// </summary>
    public W3CLoggingFields LoggingFields { get; init; } = W3CLoggingFields.All;

    /// <summary>
    ///     Maximum log file size in bytes or null for no limit.
    /// </summary>
    public int? FileSizeLimit { get; init; } = 100 * 1024 * 1024;

    /// <summary>
    ///     Maximum number of files to retain.
    /// </summary>
    public int RetainedFileCountLimit { get; init; } = 90;

    /// <summary>
    ///     Log file name.
    /// </summary>
    public string FileName { get; init; } = "access-";

    /// <summary>
    ///     Period after which the contents will get flushed to the log file.
    /// </summary>
    public TimeSpan FlushInterval { get; init; } = TimeSpan.FromSeconds(2);
}

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

/// <summary>
///     Options to influence <see cref="WebApplicationExtensions" />.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class WebApplicationOptions
{
    internal WebApplicationOptions() { }

    /// <summary>
    ///     Use UseForwardedHeaders with KnownNetworks auto-filled.
    /// </summary>
    public bool UseForwardedHeaders { get; init; } = true;

    /// <summary>
    ///     Log to access log file in W3C format.
    /// </summary>
    public bool UseW3CLogging { get; init; } = true;

    /// <summary>
    ///     Log web requests to application log as well.
    /// </summary>
    public bool UseSerilogRequestLogging { get; init; } = false;
}