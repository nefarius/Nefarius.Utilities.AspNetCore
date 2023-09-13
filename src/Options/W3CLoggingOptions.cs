using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.AspNetCore.HttpLogging;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     W3C Logging options.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public sealed class W3CLoggingOptions
{
    private const int MaxFileCount = 10000;

    private string _compressedLogsDirectory = Path.Combine(AppContext.BaseDirectory, "logs", "archived");

    private string _fileName = "access-";

    private int? _fileSizeLimit = 100 * 1024 * 1024;

    private TimeSpan _flushInterval = TimeSpan.FromSeconds(2);

    private string _logsDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

    private int _retainedCompressedFileCountLimit = 180;

    private int _retainedFileCountLimit = 6;

    internal W3CLoggingOptions() { }

    /// <summary>
    ///     Fields to include in log. Defaults to all of them.
    /// </summary>
    public W3CLoggingFields LoggingFields { get; set; } = W3CLoggingFields.All;

    /// <summary>
    ///     Maximum log file size in bytes or null for no limit. Defaults to 100 MB.
    /// </summary>
    public int? FileSizeLimit
    {
        get => _fileSizeLimit;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FileSizeLimit)} must be positive.");
            }

            _fileSizeLimit = value;
        }
    }

    /// <summary>
    ///     Maximum number of plain-text files to retain. See <see cref="CompressDeletedLogFiles" /> and
    ///     <see cref="RetainedCompressedFileCountLimit" /> to influence log compression and archived files retention. Defaults
    ///     to 3.
    /// </summary>
    public int RetainedFileCountLimit
    {
        get => _retainedFileCountLimit;
        set
        {
            if (value is <= 0 or > MaxFileCount)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(RetainedFileCountLimit)} must be between 1 and 10,000 (inclusive)");
            }

            _retainedFileCountLimit = value;
        }
    }

    /// <summary>
    ///     Log file prefix name. Defaults to "access-".
    /// </summary>
    public string FileName
    {
        get => _fileName;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _fileName = value;
        }
    }

    /// <summary>
    ///     Absolute path to directory where logs will get stored. Defaults to "logs" sub-directory within the application root
    ///     path.
    /// </summary>
    /// <remarks>
    ///     You MUST NOT set <see cref="CompressedLogsDirectory" /> to the same location, or you risk other log processors
    ///     within ASP.NET Core to accidentally delete your archived logs.
    /// </remarks>
    public string LogsDirectory
    {
        get => _logsDirectory;
        set
        {
            if (CompressedLogsDirectory == value)
            {
                throw new ArgumentException(
                    $"{nameof(LogsDirectory)} must not be equal to {nameof(CompressedLogsDirectory)}");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _logsDirectory = value;
        }
    }

    /// <summary>
    ///     Period after which the contents will get flushed to the log file. Defaults to 2 seconds.
    /// </summary>
    public TimeSpan FlushInterval
    {
        get => _flushInterval;
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FlushInterval)} must be positive.");
            }

            _flushInterval = value;
        }
    }

    /// <summary>
    ///     If set, will create a compressed archive copy of a rolled log file and deletes the original afterwards. If false,
    ///     the log processor will retain its default behaviour (just deletes older files if
    ///     <see cref="RetainedFileCountLimit" /> is hit). Defaults to true.
    /// </summary>
    public bool CompressDeletedLogFiles { get; set; } = true;

    /// <summary>
    ///     Maximum number of compressed files to retain. Set to zero to not delete any. Does nothing if
    ///     <see cref="CompressDeletedLogFiles" /> is false. Defaults to 90.
    /// </summary>
    public int RetainedCompressedFileCountLimit
    {
        get => _retainedCompressedFileCountLimit;
        set
        {
            if (value is <= 0 or > MaxFileCount)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(RetainedCompressedFileCountLimit)} must be between 1 and 10,000 (inclusive)");
            }

            _retainedCompressedFileCountLimit = value;
        }
    }

    /// <summary>
    ///     Absolute path to directory where compressed/archived logs will get stored. Defaults to "logs/archived"
    ///     sub-directory within the application root path.
    /// </summary>
    /// <remarks>
    ///     You MUST NOT set <see cref="LogsDirectory" /> to the same location, or you risk other log processors
    ///     within ASP.NET Core to accidentally delete your archived logs.
    /// </remarks>
    public string CompressedLogsDirectory
    {
        get => _compressedLogsDirectory;
        set
        {
            if (LogsDirectory == value)
            {
                throw new ArgumentException(
                    $"{nameof(CompressedLogsDirectory)} must not be equal to {nameof(LogsDirectory)}");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _compressedLogsDirectory = value;
        }
    }
}