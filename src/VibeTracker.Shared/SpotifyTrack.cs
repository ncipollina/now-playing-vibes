namespace VibeTracker.Shared;

public class SpotifyTrack
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string? AlbumImageUrl { get; set; }
    public string? PreviewUrl { get; set; }
    public string SpotifyUrl { get; set; } = string.Empty;
    public int DurationMs { get; set; }
    public int Popularity { get; set; }
}