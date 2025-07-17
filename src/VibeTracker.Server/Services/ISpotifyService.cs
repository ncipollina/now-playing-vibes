using SpotifyAPI.Web;
using VibeTracker.Shared;

namespace VibeTracker.Server.Services;

public interface ISpotifyService
{
    string GetAuthorizationUrl(string state);
    Task<string> ExchangeCodeForTokenAsync(string code, string state);
    Task<IEnumerable<SpotifyTrack>> SearchByVibeAsync(string vibeType, string? accessToken = null);
    Task<IEnumerable<SpotifyPlaylist>> GetPlaylistsByVibeAsync(string vibeType, string? accessToken = null);
}