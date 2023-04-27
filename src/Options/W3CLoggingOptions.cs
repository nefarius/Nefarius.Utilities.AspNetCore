using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.AspNetCore.HttpLogging;

namespace Nefarius.Utilities.AspNetCore.Options;

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
    ///     Absolute path to directory where logs will get stored.
    /// </summary>
    public string LogsDirectory { get; init; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    ///     Period after which the contents will get flushed to the log file.
    /// </summary>
    public TimeSpan FlushInterval { get; init; } = TimeSpan.FromSeconds(2);
}