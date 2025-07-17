# 🎵 Now Playing Vibes - Startup Guide

This guide helps you run the Now Playing Vibes application easily.

## Quick Start

### Option 1: PowerShell Script (Recommended)
```powershell
# Run this from the project root directory
./start-vibes-app.ps1
```

### Option 2: Windows Batch Script
```cmd
# Run this from the project root directory
start-vibes-app.bat
```

### Option 3: Manual Start
```bash
# Terminal 1 - Start the server
dotnet run --project src/VibeTracker.Server/VibeTracker.Server.csproj --urls http://localhost:5250

# Terminal 2 - Start the client
dotnet run --project src/VibeTracker.Client/VibeTracker.Client.csproj --urls http://localhost:5286
```

## What the Scripts Do

1. **Port Cleanup**: Kills any existing processes on ports 5250 and 5286 to prevent conflicts
2. **Build**: Compiles the entire solution to ensure everything is up-to-date
3. **Server Start**: Launches the API server on `http://localhost:5250`
4. **Client Start**: Launches the Blazor client on `http://localhost:5286`
5. **Health Check**: Verifies both services are running correctly

## Access the Application

- **Web Application**: http://localhost:5286
- **API Server**: http://localhost:5250
- **API Endpoints**:
  - GET `http://localhost:5250/vibes` - Get all vibes
  - POST `http://localhost:5250/vibes` - Submit a new vibe

## Troubleshooting

### Port Already in Use Error
The scripts handle this automatically, but if you encounter port conflicts:
- **Windows**: Use `netstat -ano | findstr :5250` to find the process, then `taskkill /f /pid <PID>`
- **macOS/Linux**: Use `lsof -ti:5250 | xargs kill -9`

### Build Errors
If you encounter build errors:
1. Make sure you have .NET 9+ SDK installed
2. Run `dotnet restore` to restore packages
3. Check for any missing dependencies

### Permission Issues
If the PowerShell script doesn't run:
```powershell
# Allow script execution (run as admin)
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

## Features

- 🎵 Submit vibes with different music genres
- 📝 View all submitted vibes in a card layout
- 🕐 Relative time display ("2 minutes ago")
- 🎨 Emoji support for different vibe types
- 📱 Responsive design with Bootstrap
- 🔄 Auto-refresh and real-time updates

## Stopping the Application

- **PowerShell Script**: Press `Ctrl+C` in the script window
- **Batch Script**: Close the server and client windows
- **Manual**: Press `Ctrl+C` in each terminal window