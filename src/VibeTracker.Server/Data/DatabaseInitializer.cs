using Dapper;
using Microsoft.Data.Sqlite;

namespace VibeTracker.Server.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS Vibes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                VibeType TEXT NOT NULL,
                Message TEXT NOT NULL,
                Timestamp TEXT NOT NULL
            );";

        await connection.ExecuteAsync(createTableSql);
    }
}