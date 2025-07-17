namespace VibeTracker.Shared;

public class SpotifyPlaylist
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string SpotifyUrl { get; set; } = string.Empty;
    public int TrackCount { get; set; }
    public string Owner { get; set; } = string.Empty;
}