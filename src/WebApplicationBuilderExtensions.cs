using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File.Archive;
using Serilog.Sinks.SystemConsole.Themes;

namespace Nefarius.Utilities.AspNetCore;

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
                rollingInterval: RollingInterval.Day,
                hooks: new ArchiveHooks(CompressionLevel.SmallestSize)
            )
            .CreateLogger();

        // logger instance used by non-DI-code
        Log.Logger = logger;

        builder.Host.UseSerilog(logger);

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