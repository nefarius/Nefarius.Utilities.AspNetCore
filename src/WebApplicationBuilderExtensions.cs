#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Nefarius.Utilities.AspNetCore.Internal;
using Nefarius.Utilities.AspNetCore.Options;
using Nefarius.Utilities.AspNetCore.Util;

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
        Action<WebApplicationBuilderOptions>? configure = null)
    {
        WebApplicationBuilderOptions options =
            builder.Configuration
                .GetSection(nameof(WebApplicationBuilderOptions))
                .Get<WebApplicationBuilderOptions>()
            ?? new WebApplicationBuilderOptions();

        options.Configuration = builder.Configuration;
        options.Environment = builder.Environment;
        options.WebHost = builder.WebHost;
        options.Host = builder.Host;

        configure?.Invoke(options);

        // apply patch that alters rolling file logic
        if (options.W3C.CompressDeletedLogFiles)
        {
            W3CLoggerPatcher.RetainedCompressedFileCountLimit = options.W3C.RetainedCompressedFileCountLimit;
            W3CLoggerPatcher.CompressedLogsDirectory = options.W3C.CompressedLogsDirectory;
            W3CLoggerPatcher.Patch();
        }

        Logger? logger = null;

        if (options.Serilog.UseSerilog)
        {
            LoggerConfiguration loggerConfiguration = options.Serilog.Configuration;

            // apply some overrides that makes logs less noisy
            foreach ((string scope, LogEventLevel level) in options.Serilog.DefaultOverrides)
            {
                loggerConfiguration.MinimumLevel.Override(scope, level);
            }

            // load additional config from settings file(s)  
            if (options.Serilog.ReadFromConfiguration)
            {
                loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
            }

            // self-explanatory ;)
            if (options.Serilog.WriteToConsole)
            {
                loggerConfiguration.WriteTo.Console(
                    applyThemeToRedirectedOutput: true,
                    theme: AnsiConsoleTheme.Literate
                );
            }

            // self-explanatory ;)
            if (options.Serilog.WriteToFile)
            {
                loggerConfiguration.WriteTo.File(
                    Path.Combine(options.Serilog.LogsDirectory, options.Serilog.ServerLogFileName),
                    rollingInterval: RollingInterval.Day,
                    hooks: new ArchiveHooks(CompressionLevel.SmallestSize)
                );
            }

            logger = loggerConfiguration.CreateLogger();

            // logger instance used by non-DI-code
            Log.Logger = logger;

            builder.Host.UseSerilog(logger);
        }

        if (options.W3C.UseW3CLogging)
        {
            // save separate access log for analysis
            builder.Services.AddW3CLogging(logging =>
            {
                logging.LoggingFields = options.W3C.LoggingFields;
                logging.FileSizeLimit = options.W3C.FileSizeLimit;
                logging.RetainedFileCountLimit = options.W3C.RetainedFileCountLimit;
                logging.FileName = options.W3C.FileName;
                logging.LogDirectory = options.W3C.LogsDirectory;
                logging.FlushInterval = options.W3C.FlushInterval;
            });
        }

        // forwarding header options
        builder.Services.Configure<ForwardedHeadersOptions>(headerOptions =>
        {
            headerOptions.ForwardedHeaders = ForwardedHeaders.All;
            headerOptions.RequireHeaderSymmetry = false;
            headerOptions.ForwardLimit = null;

            // this is convenient if e.g. you run an SSL-offloading reverse proxy on your network
            // when used with Docker, the container IP of the reverse proxy can change so auto-detect fixes that
            if (options.Forwarding.AutoDetectPrivateNetworks)
            {
                headerOptions.KnownProxies.Clear();
                headerOptions.KnownNetworks.Clear();

                foreach (var proxy in NetworkUtil.GetNetworks())
                {
                    logger?.ForContext<WebApplicationBuilderOptions>()
                        .Information("Adding known network {Subnet}", proxy);
                    headerOptions.KnownNetworks.Add(
                        new IPNetwork(proxy.BaseAddress, proxy.PrefixLength));
                }
            }

            if (options.Forwarding.AllowFromAny)
            {
                // clearing skips safety checks in the middleware
                headerOptions.KnownProxies.Clear();
                headerOptions.KnownNetworks.Clear();
            }
        });

        return builder;
    }
}