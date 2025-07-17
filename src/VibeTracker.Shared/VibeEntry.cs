namespace VibeTracker.Shared;

public class VibeEntry
{
    public int Id { get; set; }
    public string VibeType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}