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

The actual project structure is:
```
VibeTracker.sln
├── src/
│   ├── VibeTracker.Server/     # Minimal API + FastEndpoints + Dapper
│   │   ├── Data/               # Database initialization and repository
│   │   ├── Endpoints/          # FastEndpoints API endpoints
│   │   └── Program.cs          # Application startup and configuration
│   ├── VibeTracker.Client/     # Blazor WebAssembly frontend
│   │   ├── Components/Pages/   # Blazor pages (SubmitVibe, ViewVibes)
│   │   ├── Layout/            # Navigation and layout components
│   │   └── Program.cs         # Client app configuration
│   └── VibeTracker.Shared/     # Shared DTOs
│       ├── VibeRequest.cs     # Request model for submitting vibes
│       └── VibeEntry.cs       # Response model for vibe entries
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

- CORS is enabled in backend for Blazor client communication
- Uses `IVibeRepository` pattern with Dapper for data access
- Database initialization runs on app startup via `CREATE TABLE IF NOT EXISTS`
- Repository and endpoints are registered in `Program.cs`
- `NavMenu.razor` includes navigation links for Submit Vibe and View Vibes pages
- SQLite database file (`vibes.db`) is created in the server project directory
- Client connects to server on `http://localhost:5250` (configurable in `Program.cs`)
- Server runs on port 5249 by default (configurable in `launchSettings.json`)

## Running the Application

1. **Start the Server**: `dotnet run --project src/VibeTracker.Server`
2. **Start the Client**: `dotnet run --project src/VibeTracker.Client`
3. **Access the App**: Navigate to the client URL (typically `http://localhost:5285`)

## Features Implemented

- ✅ Submit Vibe page with dropdown selection and message input
- ✅ View Vibes page with card-based layout and timestamps
- ✅ Emoji support for different vibe types
- ✅ Relative time display ("2 minutes ago", "1 hour ago", etc.)
- ✅ Responsive Bootstrap styling
- ✅ Form validation and loading states
- ✅ Auto-navigation after successful submission
- ✅ Error handling for API failures