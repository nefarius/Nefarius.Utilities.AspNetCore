# WebApplicationOptions

Namespace: Nefarius.Utilities.AspNetCore.Options

Options to influence [WebApplicationExtensions](./nefarius.utilities.aspnetcore.webapplicationextensions.md).

```csharp
public sealed class WebApplicationOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [WebApplicationOptions](./nefarius.utilities.aspnetcore.options.webapplicationoptions.md)

## Properties

### <a id="properties-useforwardedheaders"/>**UseForwardedHeaders**

Use UseForwardedHeaders with [ForwardingOptions](./nefarius.utilities.aspnetcore.options.forwardingoptions.md) populated.

```csharp
public bool UseForwardedHeaders { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-useserilogrequestlogging"/>**UseSerilogRequestLogging**

Log web requests to application log as well.

```csharp
public bool UseSerilogRequestLogging { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Constructors

### <a id="constructors-.ctor"/>**WebApplicationOptions()**

```csharp
public WebApplicationOptions()
```
