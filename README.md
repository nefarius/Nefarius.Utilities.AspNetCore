<img src="assets/NSS-128x128.png" align="right" />

# Nefarius.Utilities.AspNetCore

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/nefarius/Nefarius.Utilities.AspNetCore/dotnet.yml) ![Requirements](https://img.shields.io/badge/Requires-.NET%20%3E%3D6.0-blue.svg) [![Nuget](https://img.shields.io/nuget/v/Nefarius.Utilities.AspNetCore)](https://www.nuget.org/packages/Nefarius.Utilities.AspNetCore/) ![Nuget](https://img.shields.io/nuget/dt/Nefarius.Utilities.AspNetCore)

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

## 3rd party credits

- [MonoMod](https://github.com/MonoMod/MonoMod)
- [Serilog.Sinks.File.Archive](https://github.com/cocowalla/serilog-sinks-file-archive)
- [IPNetwork2](https://www.nuget.org/packages/IPNetwork2)
