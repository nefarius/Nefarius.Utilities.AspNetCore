using System;
using System.Collections.Generic;
using System.IO;

using Serilog.Events;

namespace Nefarius.Utilities.AspNetCore.Options;

/// <summary>
///     Options to influence Serilog behaviour.
/// </summary>
public sealed class SerilogOptions
{
    /// <summary>
    ///     Absolute path to directory where logs will get stored.
    /// </summary>
    public string LogsDirectory { get; init; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    ///     Application (server) log file name.
    /// </summary>
    public string ServerLogFileName { get; init; } = "server-.log";

    /// <summary>
    ///     A set of log level overrides applied by default.
    /// </summary>
    /// <remarks>Clear this dictionary to drop all overrides, or add/replace your own.</remarks>
    public Dictionary<string, LogEventLevel> DefaultOverrides { get; } = new()
    {
        { "Microsoft.AspNetCore", LogEventLevel.Warning },
        { "Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning },
        { "Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Warning },
        { "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogEventLevel.Warning },
        { "Microsoft.Extensions.Http", LogEventLevel.Warning },
        { "System.Net.Http.HttpClient", LogEventLevel.Warning }
    };

    /// <summary>
    ///     If set, reads and merges configuration from appsettings.*.json files.
    /// </summary>
    public bool ReadFromConfiguration { get; init; } = true;

    /// <summary>
    ///     If set, writes logs to the console.
    /// </summary>
    /// <remarks>
    ///     Make sure to disable this if you have a duplicate entry in your settings and have
    ///     <see cref="ReadFromConfiguration" />enabled.
    /// </remarks>
    public bool WriteToConsole { get; init; } = true;

    /// <summary>
    ///     If set, writes log to log file with name <see cref="ServerLogFileName" /> in <see cref="LogsDirectory" />.
    /// </summary>
    /// <remarks>
    ///     Make sure to disable this if you have a duplicate entry in your settings and have
    ///     <see cref="ReadFromConfiguration" />enabled.
    /// </remarks>
    public bool WriteToFile { get; init; } = true;
}