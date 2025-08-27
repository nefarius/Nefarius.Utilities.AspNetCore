using FastEndpoints;

namespace TestWebApp;

public sealed class IndexEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return Send.OkAsync("Hello!", ct);
    }
}