using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

using Nefarius.Utilities.AspNetCore.Util;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File.Archive;
using Serilog.Sinks.SystemConsole.Themes;

using IPNetwork = System.Net.IPNetwork;

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

        // forwarding header options
        builder.Services.Configure<ForwardedHeadersOptions>(headerOptions =>
        {
            headerOptions.ForwardedHeaders = ForwardedHeaders.All;
            headerOptions.RequireHeaderSymmetry = false;
            headerOptions.ForwardLimit = null;

            if (options.AutoDetectPrivateNetworks)
            {
                foreach (IPNetwork proxy in NetworkUtil.GetNetworks(NetworkInterfaceType.Ethernet))
                {
                    logger.Information("Adding known network {Subnet}", proxy);
                    headerOptions.KnownNetworks.Add(
                        new Microsoft.AspNetCore.HttpOverrides.IPNetwork(proxy.Network, proxy.Cidr));
                }
            }
        });

        return builder;
    }
}