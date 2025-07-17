using VibeTracker.Shared;

namespace VibeTracker.Server.Data;

public interface IVibeRepository
{
    Task<IEnumerable<VibeEntry>> GetAllVibesAsync();
    Task<int> AddVibeAsync(VibeRequest vibeRequest);
}