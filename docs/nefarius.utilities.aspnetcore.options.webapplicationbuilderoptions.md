# WebApplicationBuilderOptions

Namespace: Nefarius.Utilities.AspNetCore.Options

Options to influence [WebApplicationBuilderExtensions](./nefarius.utilities.aspnetcore.webapplicationbuilderextensions.md).

```csharp
public sealed class WebApplicationBuilderOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [WebApplicationBuilderOptions](./nefarius.utilities.aspnetcore.options.webapplicationbuilderoptions.md)

## Properties

### <a id="properties-configuration"/>**Configuration**

A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.

```csharp
public ConfigurationManager Configuration { get; internal set; }
```

#### Property Value

ConfigurationManager<br>

### <a id="properties-environment"/>**Environment**

Provides information about the web hosting environment an application is running.

```csharp
public IWebHostEnvironment Environment { get; internal set; }
```

#### Property Value

IWebHostEnvironment<br>

### <a id="properties-forwarding"/>**Forwarding**

Forwarding options.

```csharp
public ForwardingOptions Forwarding { get; }
```

#### Property Value

[ForwardingOptions](./nefarius.utilities.aspnetcore.options.forwardingoptions.md)<br>

### <a id="properties-serilog"/>**Serilog**

Serilog logging options.

```csharp
public SerilogOptions Serilog { get; }
```

#### Property Value

[SerilogOptions](./nefarius.utilities.aspnetcore.options.serilogoptions.md)<br>

### <a id="properties-w3c"/>**W3C**

W3C logging options.

```csharp
public W3CLoggingOptions W3C { get; }
```

#### Property Value

[W3CLoggingOptions](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**WebApplicationBuilderOptions()**

```csharp
public WebApplicationBuilderOptions()
```
