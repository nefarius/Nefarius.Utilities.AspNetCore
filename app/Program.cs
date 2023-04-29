using FastEndpoints;

using Nefarius.Utilities.AspNetCore;

var builder = WebApplication.CreateBuilder().Setup(options =>
{
    options.W3C.RetainedFileCountLimit = 2;
});

builder.Services.AddFastEndpoints();

var app = builder.Build().Setup();

app.UseAuthorization();
app.UseFastEndpoints();

app.Run();