# WebApplicationExtensions

Namespace: Nefarius.Utilities.AspNetCore

Extensions for [WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication).

```csharp
public static class WebApplicationExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [WebApplicationExtensions](./nefarius.utilities.aspnetcore.webapplicationextensions.md)<br>
Attributes [ExtensionAttribute](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.extensionattribute)

## Methods

### <a id="methods-setup"/>**Setup(WebApplication, Action&lt;WebApplicationOptions&gt;?)**

Configures reverse proxy detection, logging, etc. Always loads [WebApplicationOptions](./nefarius.utilities.aspnetcore.options.webapplicationoptions.md) from configuration, then invokes the optional `configure` callback, which may override those loaded values.

```csharp
public static WebApplication Setup(WebApplication app, Action<WebApplicationOptions>? configure = null)
```

#### Parameters

`app` [WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication)<br>

`configure` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)<[WebApplicationOptions](./nefarius.utilities.aspnetcore.options.webapplicationoptions.md)>?<br>
Optional. Invoked after options are loaded from configuration and may override those values.

#### Returns

[WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication)
