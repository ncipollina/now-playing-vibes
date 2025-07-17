using FastEndpoints;
using VibeTracker.Server.Data;
using VibeTracker.Shared;

namespace VibeTracker.Server.Endpoints;

public class GetVibesEndpoint : EndpointWithoutRequest<IEnumerable<VibeEntry>>
{
    private readonly IVibeRepository _repository;

    public GetVibesEndpoint(IVibeRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/vibes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var vibes = await _repository.GetAllVibesAsync();
        await SendOkAsync(vibes);
    }
}