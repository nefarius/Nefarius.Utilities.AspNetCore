using FastEndpoints;

using Nefarius.Utilities.AspNetCore;

var builder = WebApplication.CreateBuilder().Setup();

builder.Services.AddFastEndpoints();

var app = builder.Build().Setup();

app.UseAuthorization();
app.UseFastEndpoints();

app.Run();