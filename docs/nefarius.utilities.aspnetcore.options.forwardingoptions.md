# ForwardingOptions

Namespace: Nefarius.Utilities.AspNetCore.Options

Options to influence HTTP header forwarding behaviour.

```csharp
public sealed class ForwardingOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ForwardingOptions](./nefarius.utilities.aspnetcore.options.forwardingoptions.md)

## Properties

### <a id="properties-allowfromany"/>**AllowFromAny**

If set, all remote addresses will be treated as a proxy and therefore Forwarded For headers will be parsed.

```csharp
public bool AllowFromAny { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
This can not be enabled if [ForwardingOptions.AutoDetectPrivateNetworks](./nefarius.utilities.aspnetcore.options.forwardingoptions.md#autodetectprivatenetworks) is enabled.

**Remarks:**

Only enable this if the service is run behind a reverse proxy, otherwise header spoofing is a possibility!

### <a id="properties-autodetectprivatenetworks"/>**AutoDetectPrivateNetworks**

If set, will auto-detect local networks and add them as known networks for forwarding header options.

```csharp
public bool AutoDetectPrivateNetworks { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
This can not be enabled if [ForwardingOptions.AllowFromAny](./nefarius.utilities.aspnetcore.options.forwardingoptions.md#allowfromany) is enabled.

**Remarks:**

Only enable this if the service is run behind a reverse proxy, otherwise header spoofing is a possibility!
