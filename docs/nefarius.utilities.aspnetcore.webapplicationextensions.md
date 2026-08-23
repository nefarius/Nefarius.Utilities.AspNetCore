# WebApplicationExtensions

Namespace: Nefarius.Utilities.AspNetCore

Extensions for [WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication).

```csharp
public static class WebApplicationExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [WebApplicationExtensions](./nefarius.utilities.aspnetcore.webapplicationextensions.md)<br>
Attributes [ExtensionAttribute](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.extensionattribute)

## Methods

### <a id="methods-setup"/>**Setup(WebApplication, Action&lt;WebApplicationOptions&gt;)**

Configures reverse proxy detection, logging, etc.

```csharp
public static WebApplication Setup(WebApplication app, Action<WebApplicationOptions> configure)
```

#### Parameters

`app` [WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication)<br>

`configure` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)<[WebApplicationOptions](./nefarius.utilities.aspnetcore.options.webapplicationoptions.md)><br>

#### Returns

[WebApplication](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplication)
