using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

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
}

/// <summary>
///     Extensions for <see cref="WebApplicationBuilder" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Configures logging and other basic services.
    /// </summary>
    public static WebApplicationBuilder Setup(this WebApplicationBuilder builder,
        Action<WebApplicationBuilderOptions> configure = default)
    {
        WebApplicationBuilderOptions options = new();

        configure?.Invoke(options);

        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            // required to have colored output in Docker
            .WriteTo.Console(
                applyThemeToRedirectedOutput: true,
                theme: AnsiConsoleTheme.Literate
            )
            .WriteTo.File(
                Path.Combine(options.LogsDirectory, options.ServerLogFileName),
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();

        // logger instance used by non-DI-code
        Log.Logger = logger;

        builder.Host.UseSerilog(logger);

        builder.Services.AddLogging(b =>
        {
            b.SetMinimumLevel(LogLevel.Information);
            b.AddSerilog(logger, true);
        });

        builder.Services.AddSingleton(new LoggerFactory().AddSerilog(logger));

        // save separate access log for analysis
        builder.Services.AddW3CLogging(logging =>
        {
            logging.LoggingFields = options.W3C.LoggingFields;
            logging.FileSizeLimit = options.W3C.FileSizeLimit;
            logging.RetainedFileCountLimit = options.W3C.RetainedFileCountLimit;
            logging.FileName = options.W3C.FileName;
            logging.LogDirectory = options.LogsDirectory;
            logging.FlushInterval = options.W3C.FlushInterval;
        });

        return builder;
    }
}