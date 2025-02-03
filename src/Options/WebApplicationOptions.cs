using System.Diagnostics.CodeAnalysis;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     Options to influence <see cref="WebApplicationExtensions" />.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class WebApplicationOptions
{
    /// <summary>
    ///     Use UseForwardedHeaders with <see cref="ForwardingOptions"/> populated.
    /// </summary>
    public bool UseForwardedHeaders { get; set; } = true;

    /// <summary>
    ///     Log web requests to application log as well.
    /// </summary>
    public bool UseSerilogRequestLogging { get; set; } = false;
}