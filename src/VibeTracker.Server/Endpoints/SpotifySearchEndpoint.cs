using FastEndpoints;
using VibeTracker.Server.Services;

namespace VibeTracker.Server.Endpoints;

public class SpotifySearchRequest
{
    public string VibeType { get; set; } = string.Empty;
}

public class SpotifySearchEndpoint : Endpoint<SpotifySearchRequest>
{
    private readonly ISpotifyService _spotifyService;

    public SpotifySearchEndpoint(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public override void Configure()
    {
        Get("/spotify/search/{vibeType}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SpotifySearchRequest request, CancellationToken ct)
    {
        try
        {
            var accessToken = HttpContext.Session.GetString("spotify_access_token");
            
            var tracks = await _spotifyService.SearchByVibeAsync(request.VibeType, accessToken);
            var playlists = await _spotifyService.GetPlaylistsByVibeAsync(request.VibeType, accessToken);
            
            await SendOkAsync(new { tracks, playlists });
        }
        catch (Exception ex)
        {
            await SendAsync(new { error = "Failed to search Spotify: " + ex.Message }, 500);
        }
    }
}

public class SpotifyUserEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/spotify/user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var accessToken = HttpContext.Session.GetString("spotify_access_token");
        var isConnected = !string.IsNullOrEmpty(accessToken);
        
        await SendOkAsync(new { isConnected, hasToken = isConnected });
    }
}