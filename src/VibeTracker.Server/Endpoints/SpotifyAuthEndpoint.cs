using FastEndpoints;
using Microsoft.Extensions.Caching.Memory;
using VibeTracker.Server.Services;

namespace VibeTracker.Server.Endpoints;

public class SpotifyAuthEndpoint : EndpointWithoutRequest
{
    private readonly ISpotifyService _spotifyService;
    private readonly IMemoryCache _cache;

    public SpotifyAuthEndpoint(ISpotifyService spotifyService, IMemoryCache cache)
    {
        _spotifyService = spotifyService;
        _cache = cache;
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
        
        // Store state in memory cache for validation (more reliable than sessions for OAuth)
        var cacheKey = $"spotify_state_{state}";
        _cache.Set(cacheKey, state, TimeSpan.FromMinutes(10)); // 10 minute expiry
        
        // Debug logging
        Console.WriteLine($"DEBUG AUTH: Generated state: '{state}'");
        Console.WriteLine($"DEBUG AUTH: Cache key: '{cacheKey}'");
        Console.WriteLine($"DEBUG AUTH: Stored state in cache: '{_cache.Get(cacheKey)}'");
        
        await SendOkAsync(new { authUrl, state });
    }
}

public class SpotifyCallbackRequest
{
    public string? Code { get; set; }
    public string? State { get; set; }
    public string? Error { get; set; }
}

public class SpotifyCallbackEndpoint : Endpoint<SpotifyCallbackRequest>
{
    private readonly ISpotifyService _spotifyService;
    private readonly IMemoryCache _cache;

    public SpotifyCallbackEndpoint(ISpotifyService spotifyService, IMemoryCache cache)
    {
        _spotifyService = spotifyService;
        _cache = cache;
    }

    public override void Configure()
    {
        Get("/callback/spotify");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SpotifyCallbackRequest req, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(req.Error))
        {
            await SendAsync(new { error = "Spotify authorization failed: " + req.Error }, 400);
            return;
        }

        if (string.IsNullOrEmpty(req.Code) || string.IsNullOrEmpty(req.State))
        {
            await SendAsync(new { error = "Missing code or state parameter" }, 400);
            return;
        }

        // Validate state using memory cache
        var cacheKey = $"spotify_state_{req.State}";
        var cachedState = _cache.Get<string>(cacheKey);
        
        // Debug logging
        Console.WriteLine($"DEBUG: Cache key: '{cacheKey}'");
        Console.WriteLine($"DEBUG: Cached state: '{cachedState}'");
        Console.WriteLine($"DEBUG: Received state: '{req.State}'");
        
        if (cachedState != req.State)
        {
            await SendAsync(new { 
                error = "Invalid state parameter",
                debug = new {
                    cachedState = cachedState,
                    receivedState = req.State,
                    cacheKey = cacheKey,
                    stateFound = cachedState != null
                }
            }, 400);
            return;
        }
        
        // Remove the state from cache after successful validation (one-time use)
        _cache.Remove(cacheKey);

        try
        {
            var accessToken = await _spotifyService.ExchangeCodeForTokenAsync(req.Code, req.State);
            
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