using FastEndpoints;

using Nefarius.Utilities.AspNetCore;

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

builder.Services.AddFastEndpoints();

var app = builder.Build().Setup();

app.UseAuthorization();
app.UseFastEndpoints();

app.Run();