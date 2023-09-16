using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

using Serilog;

using WebApplicationOptions = Nefarius.Utilities.AspNetCore.Options.WebApplicationOptions;

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
        WebApplicationOptions options =
            app.Configuration
                .GetSection(nameof(WebApplicationOptions))
                .Get<WebApplicationOptions>()
            ?? new WebApplicationOptions();

        configure?.Invoke(options);

        // required for log rotation with compression to work properly
        app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

        if (options.UseForwardedHeaders)
        {
            // this must come first or the wrong client IPs end up in the logs
            app.UseForwardedHeaders();
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