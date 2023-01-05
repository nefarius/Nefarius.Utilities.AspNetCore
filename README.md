<img src="assets/NSS-128x128.png" align="right" />

# Nefarius.Utilities.AspNetCore

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/nefarius/Nefarius.Utilities.AspNetCore/dotnet.yml) ![Nuget](https://img.shields.io/nuget/dt/Nefarius.Utilities.AspNetCore)

My opinionated collection of utilities for ASP.NET Core applications.

## Features

- Sets up logging with [Serilog](https://github.com/serilog/serilog-aspnetcore)
- Sets up log file rotation and [compression](https://github.com/cocowalla/serilog-sinks-file-archive)
- Sets up [forwarded headers](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.forwardedheadersextensions.useforwardedheaders?view=aspnetcore-7.0) and auto-configures local networks so the correct client IP ends up in logs and middleware

## How to use

Replace

```cs
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
```

with

```cs
WebApplicationBuilder builder = WebApplication.CreateBuilder(args).Setup();
```

and

```cs
WebApplication app = builder.Build();
```

with

```cs
WebApplication app = builder.Build().Setup();
```

and you're all set!
