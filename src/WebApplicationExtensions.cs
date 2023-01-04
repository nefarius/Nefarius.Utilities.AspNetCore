using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

using Nefarius.Utilities.AspNetCore.Util;

using Serilog;

namespace Nefarius.Utilities.AspNetCore;

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
