# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a full-stack music-themed application called "Now Playing Vibes" built with:
- **Frontend**: Blazor WebAssembly
- **Backend**: .NET 9 Minimal API using FastEndpoints  
- **Data Access**: Dapper
- **Database**: SQLite (file-based)
- **IDE**: JetBrains Rider

The app allows users to submit "vibes" (mood categories with messages about what they're listening to) and view all submitted vibes.

## Project Structure

The intended project structure is:
```
VibeTracker.sln
├── src/
│   ├── VibeTracker.Server/     # Minimal API + FastEndpoints + Dapper
│   ├── VibeTracker.Client/     # Blazor WebAssembly frontend  
│   └── VibeTracker.Shared/     # Shared DTOs (optional)
```

## Development Commands

Since this is a .NET project, use standard .NET CLI commands:

```bash
# Create solution and projects
dotnet new sln -n VibeTracker
dotnet new blazorwasm -n VibeTracker.Client -o src/VibeTracker.Client
dotnet new web -n VibeTracker.Server -o src/VibeTracker.Server
dotnet new classlib -n VibeTracker.Shared -o src/VibeTracker.Shared

# Add projects to solution
dotnet sln add src/VibeTracker.Client/VibeTracker.Client.csproj
dotnet sln add src/VibeTracker.Server/VibeTracker.Server.csproj
dotnet sln add src/VibeTracker.Shared/VibeTracker.Shared.csproj

# Build and run
dotnet build
dotnet run --project src/VibeTracker.Server
dotnet run --project src/VibeTracker.Client

# Run tests (if any)
dotnet test
```

## Key Architecture Components

### Backend API (FastEndpoints)
- **POST `/vibes`**: Accepts `VibeRequest` with VibeType and Message
- **GET `/vibes`**: Returns all `VibeEntry` objects with Id, VibeType, Message, and Timestamp

### Database Schema
SQLite table created on startup:
```sql
CREATE TABLE IF NOT EXISTS Vibes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    VibeType TEXT NOT NULL,
    Message TEXT NOT NULL,
    Timestamp TEXT NOT NULL
);
```

### Frontend Pages
- **Submit Vibe Page**: Dropdown for vibe selection + textbox for activity/music
- **View Vibes Page**: Displays all vibes as cards or table with timestamp

### Required NuGet Packages
- `Microsoft.Data.Sqlite` (SQLite support)
- `FastEndpoints` (API framework)
- `Dapper` (data access)

## Implementation Notes

- Enable CORS in backend for Blazor client communication
- Use `IVibeRepository` pattern with Dapper for data access
- Database initialization runs on app startup via `CREATE TABLE IF NOT EXISTS`
- Repository and endpoints should be registered in `Program.cs`
- Update `NavMenu.razor` to include navigation links for both pages