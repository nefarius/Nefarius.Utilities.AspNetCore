# SerilogOptions

Namespace: Nefarius.Utilities.AspNetCore.Options

Options to influence Serilog behaviour.

```csharp
public sealed class SerilogOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [SerilogOptions](./nefarius.utilities.aspnetcore.options.serilogoptions.md)

## Properties

### <a id="properties-configuration"/>**Configuration**

The default Serilog  to use.

```csharp
public LoggerConfiguration Configuration { get; }
```

#### Property Value

LoggerConfiguration<br>

### <a id="properties-defaultoverrides"/>**DefaultOverrides**

A set of log level overrides applied by default.

```csharp
public Dictionary<String, LogEventLevel> DefaultOverrides { get; }
```

#### Property Value

[Dictionary&lt;String, LogEventLevel&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)<br>

**Remarks:**

Clear this dictionary to drop all overrides, or add/replace your own.

### <a id="properties-logsdirectory"/>**LogsDirectory**

Absolute path to directory where logs will get stored.

```csharp
public string LogsDirectory { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-readfromconfiguration"/>**ReadFromConfiguration**

If set, reads and merges configuration from appsettings.*.json files.

```csharp
public bool ReadFromConfiguration { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-serverlogfilename"/>**ServerLogFileName**

Application (server) log file name.

```csharp
public string ServerLogFileName { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-writetoconsole"/>**WriteToConsole**

If set, writes logs to the console.

```csharp
public bool WriteToConsole { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

**Remarks:**

Make sure to disable this if you have a duplicate entry in your settings and have
 [SerilogOptions.ReadFromConfiguration](./nefarius.utilities.aspnetcore.options.serilogoptions.md#readfromconfiguration)enabled.

### <a id="properties-writetofile"/>**WriteToFile**

If set, writes log to log file with name [SerilogOptions.ServerLogFileName](./nefarius.utilities.aspnetcore.options.serilogoptions.md#serverlogfilename) in [SerilogOptions.LogsDirectory](./nefarius.utilities.aspnetcore.options.serilogoptions.md#logsdirectory).

```csharp
public bool WriteToFile { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

**Remarks:**

Make sure to disable this if you have a duplicate entry in your settings and have
 [SerilogOptions.ReadFromConfiguration](./nefarius.utilities.aspnetcore.options.serilogoptions.md#readfromconfiguration)enabled.

## Constructors

### <a id="constructors-.ctor"/>**SerilogOptions()**

```csharp
public SerilogOptions()
```
