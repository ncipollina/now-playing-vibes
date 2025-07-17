namespace VibeTracker.Shared;

public class SpotifyAuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public string Scope { get; set; } = string.Empty;
}