using SpotifyAPI.Web;
using VibeTracker.Shared;

namespace VibeTracker.Server.Services;

public class SpotifyService : ISpotifyService
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> _vibeToSearchQuery;

    public SpotifyService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        // Map vibe types to Spotify search queries
        _vibeToSearchQuery = new Dictionary<string, string>
        {
            ["Lo-fi"] = "lo-fi hip hop chill study beats",
            ["Metal"] = "metal rock heavy thrash",
            ["Synthwave"] = "synthwave retro electronic 80s",
            ["Classical"] = "classical music orchestral instrumental",
            ["Jazz"] = "jazz smooth blues instrumental",
            ["Rock"] = "rock alternative indie guitar",
            ["Electronic"] = "electronic dance EDM house",
            ["Indie"] = "indie alternative rock singer-songwriter",
            ["Hip-Hop"] = "hip hop rap urban contemporary",
            ["Folk"] = "folk acoustic indie americana"
        };
    }

    public string GetAuthorizationUrl(string state)
    {
        var clientId = _configuration["Spotify:ClientId"];
        var redirectUri = _configuration["Spotify:RedirectUri"];
        
        var loginRequest = new LoginRequest(
            new Uri(redirectUri!),
            clientId!,
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[]
            {
                Scopes.UserReadPrivate,
                Scopes.UserReadEmail,
                Scopes.Streaming,
                Scopes.UserModifyPlaybackState,
                Scopes.UserReadPlaybackState
            },
            State = state
        };

        return loginRequest.ToUri().ToString();
    }

    public async Task<string> ExchangeCodeForTokenAsync(string code, string state)
    {
        var clientId = _configuration["Spotify:ClientId"];
        var clientSecret = _configuration["Spotify:ClientSecret"];
        var redirectUri = _configuration["Spotify:RedirectUri"];

        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(clientId!, clientSecret!, code, new Uri(redirectUri!))
        );

        return response.AccessToken;
    }

    public async Task<IEnumerable<SpotifyTrack>> SearchByVibeAsync(string vibeType, string? accessToken = null)
    {
        var spotify = await GetSpotifyClientAsync(accessToken);
        if (spotify == null) return Enumerable.Empty<SpotifyTrack>();

        var searchQuery = _vibeToSearchQuery.GetValueOrDefault(vibeType, vibeType);
        
        var searchRequest = new SearchRequest(SearchRequest.Types.Track, searchQuery)
        {
            Limit = 20
        };

        var searchResponse = await spotify.Search.Item(searchRequest);
        
        return searchResponse.Tracks.Items?.Select(track => new SpotifyTrack
        {
            Id = track.Id,
            Name = track.Name,
            Artist = string.Join(", ", track.Artists.Select(a => a.Name)),
            Album = track.Album.Name,
            AlbumImageUrl = track.Album.Images?.FirstOrDefault()?.Url,
            PreviewUrl = track.PreviewUrl,
            SpotifyUrl = track.ExternalUrls["spotify"],
            DurationMs = track.DurationMs,
            Popularity = track.Popularity
        }) ?? Enumerable.Empty<SpotifyTrack>();
    }

    public async Task<IEnumerable<SpotifyPlaylist>> GetPlaylistsByVibeAsync(string vibeType, string? accessToken = null)
    {
        var spotify = await GetSpotifyClientAsync(accessToken);
        if (spotify == null) return Enumerable.Empty<SpotifyPlaylist>();

        var searchQuery = _vibeToSearchQuery.GetValueOrDefault(vibeType, vibeType);
        
        var searchRequest = new SearchRequest(SearchRequest.Types.Playlist, searchQuery)
        {
            Limit = 10
        };

        var searchResponse = await spotify.Search.Item(searchRequest);
        
        return searchResponse.Playlists.Items?.Select(playlist => new SpotifyPlaylist
        {
            Id = playlist.Id!,
            Name = playlist.Name!,
            Description = playlist.Description ?? "",
            ImageUrl = playlist.Images?.FirstOrDefault()?.Url,
            SpotifyUrl = playlist.ExternalUrls!["spotify"],
            TrackCount = playlist.Tracks?.Total ?? 0,
            Owner = playlist.Owner?.DisplayName ?? "Unknown"
        }) ?? Enumerable.Empty<SpotifyPlaylist>();
    }

    private async Task<SpotifyClient?> GetSpotifyClientAsync(string? accessToken = null)
    {
        if (!string.IsNullOrEmpty(accessToken))
        {
            return new SpotifyClient(accessToken);
        }

        // For public searches, use Client Credentials flow
        var clientId = _configuration["Spotify:ClientId"];
        var clientSecret = _configuration["Spotify:ClientSecret"];

        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(clientId!, clientSecret!);
        var response = await new OAuthClient(config).RequestToken(request);

        return new SpotifyClient(response.AccessToken);
    }
}