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
    public W3CLoggingFields LoggingFields { get; set; } = W3CLoggingFields.All;

    /// <summary>
    ///     Maximum log file size in bytes or null for no limit.
    /// </summary>
    public int? FileSizeLimit { get; set; } = 100 * 1024 * 1024;

    /// <summary>
    ///     Maximum number of plain-text files to retain. See <see cref="CompressDeletedLogFiles" /> and
    ///     <see cref="RetainedCompressedFileCountLimit" /> to influence log compression and archived files retention.
    /// </summary>
    public int RetainedFileCountLimit { get; set; } = 3;

    /// <summary>
    ///     Log file prefix name.
    /// </summary>
    public string FileName { get; set; } = "access-";

    /// <summary>
    ///     Absolute path to directory where logs will get stored.
    /// </summary>
    public string LogsDirectory { get; set; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    ///     Period after which the contents will get flushed to the log file.
    /// </summary>
    public TimeSpan FlushInterval { get; set; } = TimeSpan.FromSeconds(2);

    /// <summary>
    ///     If set, will create a compressed archive copy of a rolled log file and deletes the original afterwards. If false,
    ///     the log processor will retain its default behaviour (just deletes older files if
    ///     <see cref="RetainedFileCountLimit" /> is hit).
    /// </summary>
    public bool CompressDeletedLogFiles { get; set; } = true;

    /// <summary>
    ///     Maximum number of compressed files to retain. Set to zero to not delete any. Does nothing if
    ///     <see cref="CompressDeletedLogFiles" /> is false.
    /// </summary>
    public int RetainedCompressedFileCountLimit { get; set; } = 90;
    
    /// <summary>
    ///     Absolute path to directory where compressed/archived logs will get stored.
    /// </summary>
    public string CompressedLogsDirectory { get; set; } = Path.Combine(AppContext.BaseDirectory, "logs", "archived");
}