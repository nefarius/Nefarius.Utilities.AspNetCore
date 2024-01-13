using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     Options to influence <see cref="WebApplicationBuilderExtensions" />.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class WebApplicationBuilderOptions
{
    /// <summary>
    ///     Serilog logging options.
    /// </summary>
    public SerilogOptions Serilog { get; } = new();

    /// <summary>
    ///     W3C logging options.
    /// </summary>
    public W3CLoggingOptions W3C { get; } = new();

    /// <summary>
    ///     Forwarding options.
    /// </summary>
    public ForwardingOptions Forwarding { get; } = new();

    /// <summary>
    ///     A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    public ConfigurationManager Configuration { get; internal set; } = null!;
    
    /// <summary>
    ///     Provides information about the web hosting environment an application is running.
    /// </summary>
    public IWebHostEnvironment Environment { get; internal set; } = null!;
}