using FastEndpoints;
using VibeTracker.Server.Services;

namespace VibeTracker.Server.Endpoints;

public class SpotifyAuthEndpoint : EndpointWithoutRequest
{
    private readonly ISpotifyService _spotifyService;

    public SpotifyAuthEndpoint(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public override void Configure()
    {
        Get("/spotify/auth");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var state = Guid.NewGuid().ToString();
        var authUrl = _spotifyService.GetAuthorizationUrl(state);
        
        // Store state in session for validation (simplified for demo)
        HttpContext.Session.SetString("spotify_state", state);
        
        await SendOkAsync(new { authUrl, state });
    }
}

public class SpotifyCallbackEndpoint : EndpointWithoutRequest
{
    private readonly ISpotifyService _spotifyService;

    public SpotifyCallbackEndpoint(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public override void Configure()
    {
        Get("/callback/spotify");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var code = Query<string>("code");
        var state = Query<string>("state");
        var error = Query<string>("error");

        if (!string.IsNullOrEmpty(error))
        {
            await SendAsync(new { error = "Spotify authorization failed: " + error }, 400);
            return;
        }

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            await SendAsync(new { error = "Missing code or state parameter" }, 400);
            return;
        }

        // Validate state (simplified for demo)
        var sessionState = HttpContext.Session.GetString("spotify_state");
        if (sessionState != state)
        {
            await SendAsync(new { error = "Invalid state parameter" }, 400);
            return;
        }

        try
        {
            var accessToken = await _spotifyService.ExchangeCodeForTokenAsync(code, state);
            
            // Store access token in session (in production, use secure storage)
            HttpContext.Session.SetString("spotify_access_token", accessToken);
            
            // Redirect to success page
            await SendRedirectAsync("/spotify-success");
        }
        catch (Exception ex)
        {
            await SendAsync(new { error = "Failed to exchange code for token: " + ex.Message }, 500);
        }
    }
}