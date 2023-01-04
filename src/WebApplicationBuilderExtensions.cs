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
///     Options to influence <see cref="WebApplicationBuilderExtensions"/>.
/// </summary>
public sealed class WebApplicationBuilderOptions
{
    // TODO: implement me
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

        string logsDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

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
                Path.Combine(logsDirectory, "server-.log"),
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
            // Log all W3C fields
            logging.LoggingFields = W3CLoggingFields.All;

            logging.FileSizeLimit = 100 * 1024 * 1024;
            logging.RetainedFileCountLimit = 90;
            logging.FileName = "access-";
            logging.LogDirectory = logsDirectory;
            logging.FlushInterval = TimeSpan.FromSeconds(2);
        });

        return builder;
    }
}