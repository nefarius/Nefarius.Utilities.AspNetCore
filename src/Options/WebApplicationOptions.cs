using System.Diagnostics.CodeAnalysis;

namespace Nefarius.Utilities.AspNetCore.Options;

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
    public bool UseForwardedHeaders { get; set; } = true;

    /// <summary>
    ///     Log to access log file in W3C format.
    /// </summary>
    public bool UseW3CLogging { get; set; } = true;

    /// <summary>
    ///     Log web requests to application log as well.
    /// </summary>
    public bool UseSerilogRequestLogging { get; set; } = false;
}