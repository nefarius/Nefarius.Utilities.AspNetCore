using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

using Nefarius.Utilities.AspNetCore.Util;

using Serilog;

namespace Nefarius.Utilities.AspNetCore;

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
    public bool UseSerilogRequestLogging { get; init; } = true;
}

/// <summary>
///     Extensions for <see cref="WebApplication" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class WebApplicationExtensions
{
    /// <summary>
    ///     Configures reverse proxy detection, logging, etc.
    /// </summary>
    public static WebApplication Setup(this WebApplication app, Action<WebApplicationOptions> configure = default)
    {
        WebApplicationOptions options = new();

        configure?.Invoke(options);

        if (options.UseForwardedHeaders)
        {
            ForwardedHeadersOptions headerOptions = new()
            {
                ForwardedHeaders = ForwardedHeaders.All, RequireHeaderSymmetry = false, ForwardLimit = null
            };

            foreach (IPNetwork proxy in NetworkUtil.GetNetworks(NetworkInterfaceType.Ethernet))
            {
                headerOptions.KnownNetworks.Add(proxy);
            }

            // this must come first or the wrong client IPs end up in the logs
            app.UseForwardedHeaders(headerOptions);
        }

        if (options.UseW3CLogging)
        {
            app.UseW3CLogging();
        }

        if (options.UseSerilogRequestLogging)
        {
            app.UseSerilogRequestLogging(
                opts =>
                {
                    opts.MessageTemplate =
                        "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    opts.EnrichDiagnosticContext = (
                        diagnosticContext,
                        httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                    };
                });
        }

        return app;
    }
}
