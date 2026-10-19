# WebApplicationBuilderExtensions

Namespace: Nefarius.Utilities.AspNetCore

Extensions for [WebApplicationBuilder](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder).

```csharp
public static class WebApplicationBuilderExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [WebApplicationBuilderExtensions](./nefarius.utilities.aspnetcore.webapplicationbuilderextensions.md)<br>
Attributes [ExtensionAttribute](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.extensionattribute)

## Methods

### <a id="methods-setup"/>**Setup(WebApplicationBuilder, Action&lt;WebApplicationBuilderOptions&gt;)**

Configures logging and other basic services.

```csharp
public static WebApplicationBuilder Setup(WebApplicationBuilder builder, Action<WebApplicationBuilderOptions> configure)
```

#### Parameters

`builder` [WebApplicationBuilder](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder)<br>

`configure` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)<[WebApplicationBuilderOptions](./nefarius.utilities.aspnetcore.options.webapplicationbuilderoptions.md)><br>

#### Returns

[WebApplicationBuilder](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder)
