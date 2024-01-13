<img src="assets/NSS-128x128.png" align="right" />

# Nefarius.Utilities.AspNetCore

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/nefarius/Nefarius.Utilities.AspNetCore/build.yml) ![Requirements](https://img.shields.io/badge/Requires-.NET%20%3E%3D6.0-blue.svg) [![Nuget](https://img.shields.io/nuget/v/Nefarius.Utilities.AspNetCore)](https://www.nuget.org/packages/Nefarius.Utilities.AspNetCore/) ![Nuget](https://img.shields.io/nuget/dt/Nefarius.Utilities.AspNetCore)

My opinionated collection of utilities for ASP.NET Core applications.

## Features

- Sets up application (and optionally web requests) logging with [Serilog](https://github.com/serilog/serilog-aspnetcore)
- Sets up application log file rotation and [compression](https://github.com/cocowalla/serilog-sinks-file-archive)
- Sets up [W3C logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger/) to a separate access log file
  - Compresses rolled W3C log files and allows for their own retention settings
- Sets up [forwarded headers](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.forwardedheadersextensions.useforwardedheaders?view=aspnetcore-7.0) and auto-configures local networks so the correct client IP ends up in logs and middleware
  - ⚠️ This assumes that your app sits behind a reverse proxy, **do not enable this setting** if your app faces the Internet directly or header spoofing becomes possible!
- ... and more as I start incorporating this lib in my projects!

## How to use

Replace 👇

```cs
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
```

with 👇

```cs
WebApplicationBuilder builder = WebApplication.CreateBuilder(args).Setup();
```

and 👇

```cs
WebApplication app = builder.Build();
```

with 👇

```cs
WebApplication app = builder.Build().Setup();
```

and you're all set! 👏 The `Setup` extension methods take optional configuration arguments you can provide to alter the default behaviour.

### Loading additional configuration

Let's assume you have a custom `/app/secrets/appsettings.json` file adding one or more additional sinks (MongoDB in this example):

```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.MongoDB"
    ],
    "WriteTo": [
      {
        "Name": "MongoDBBson",
        "Args": {
          "databaseUrl": "mongodb+srv://db-cluster/database?authSource=admin",
          "collectionName": "logs",
          "cappedMaxSizeMb": "1024",
          "cappedMaxDocuments": "50000",
          "rollingInterval": "Month"
        }
      }
    ]
  }
}
```

To have this configuration file read/merged you can access and modify the `Configuration` of the `WebApplicationBuilderOptions` like so:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args).Setup(opts =>
{
    // loads settings from K8s secret
    opts.Configuration.AddJsonFile("secrets/appsettings.json", optional: true);
});
```

This ensures that the Serilog sinks configuration is read early enough when the logger is created. 

## Example configurations

### From code

#### Enable and customize W3C log compression

The following settings use the library defaults, they're simply explained here and don't need to be exclusively set if you're satisfied with the defaults 😉

```csharp
var builder = WebApplication.CreateBuilder().Setup(options =>
{
    // this will only keep three most recent uncompressed log files
    options.W3C.RetainedFileCountLimit = 3;
    // on rotation, make a compressed archive copy before deleting the original
    options.W3C.CompressDeletedLogFiles = true;
    // keeps the last 90 compressed log files on top of the original files
    // after this, even the compressed logs are finally deleted from disk
    options.W3C.RetainedCompressedFileCountLimit = 90;
});
```

### From `appsettings.json`

You can also alter the defaults from your configuration; simply stick to the options classes and property naming conventions like so: 

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "WebApplicationBuilderOptions": {
    "AutoDetectPrivateNetworks": false
  },
  "WebApplicationOptions": {
    "UseForwardedHeaders": false
  }
}
```

Bear in mind that changing the same option in code will take priority over application configuration.

### From `docker-compose.yml`

Using this format you can change the settings directly in the compose file:

```yml
...
    environment:
      - TZ=Europe/Vienna
      - WebApplicationBuilderOptions__W3C__RetainedCompressedFileCountLimit=600
      - WebApplicationBuilderOptions__W3C__RetainedFileCountLimit=12
...
```

## 3rd party credits

- [MonoMod](https://github.com/MonoMod/MonoMod)
- [Serilog.Sinks.File.Archive](https://github.com/cocowalla/serilog-sinks-file-archive)
- [IPNetwork2](https://www.nuget.org/packages/IPNetwork2)
