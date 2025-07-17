using Dapper;
using Microsoft.Data.Sqlite;
using VibeTracker.Shared;

namespace VibeTracker.Server.Data;

public class VibeRepository : IVibeRepository
{
    private readonly string _connectionString;

    public VibeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<VibeEntry>> GetAllVibesAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            SELECT Id, VibeType, Message, Timestamp 
            FROM Vibes 
            ORDER BY Timestamp DESC";

        var vibes = await connection.QueryAsync<VibeEntry>(sql);
        
        // Convert string timestamps back to DateTime
        foreach (var vibe in vibes)
        {
            if (DateTime.TryParse(vibe.Timestamp.ToString(), out var timestamp))
            {
                vibe.Timestamp = timestamp;
            }
        }

        return vibes;
    }

    public async Task<int> AddVibeAsync(VibeRequest vibeRequest)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO Vibes (VibeType, Message, Timestamp) 
            VALUES (@VibeType, @Message, @Timestamp);
            SELECT last_insert_rowid();";

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        
        var id = await connection.QuerySingleAsync<int>(sql, new
        {
            VibeType = vibeRequest.VibeType,
            Message = vibeRequest.Message,
            Timestamp = timestamp
        });

        return id;
    }
}