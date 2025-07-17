using FastEndpoints;
using VibeTracker.Server.Data;
using VibeTracker.Shared;

namespace VibeTracker.Server.Endpoints;

public class CreateVibeEndpoint : Endpoint<VibeRequest>
{
    private readonly IVibeRepository _repository;

    public CreateVibeEndpoint(IVibeRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Post("/vibes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(VibeRequest request, CancellationToken ct)
    {
        var id = await _repository.AddVibeAsync(request);
        await SendOkAsync(new { Id = id });
    }
}