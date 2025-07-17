@echo off
title Now Playing Vibes Application

echo.
echo 🎵 Starting Now Playing Vibes Application...
echo =================================================

REM Check if we're in the correct directory
if not exist "VibeTracker.sln" (
    echo ❌ Error: VibeTracker.sln not found. Please run this script from the project root directory.
    pause
    exit /b 1
)

REM Kill any existing processes on the ports we'll use
echo 🧹 Cleaning up existing processes...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5250') do taskkill /f /pid %%a >nul 2>&1
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5286') do taskkill /f /pid %%a >nul 2>&1
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5249') do taskkill /f /pid %%a >nul 2>&1
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5285') do taskkill /f /pid %%a >nul 2>&1

REM Build the solution first
echo 🔨 Building the solution...
dotnet build
if %errorlevel% neq 0 (
    echo ❌ Build failed! Please fix the build errors and try again.
    pause
    exit /b 1
)

echo ✅ Build successful!

REM Start the server in a new window
echo 🚀 Starting the API server...
start "VibeTracker Server" cmd /c "dotnet run --project src/VibeTracker.Server/VibeTracker.Server.csproj --urls http://localhost:5250"

REM Wait for server to start
echo ⏳ Waiting for server to start...
timeout /t 5 /nobreak >nul

REM Start the client in a new window
echo 🚀 Starting the Blazor client...
start "VibeTracker Client" cmd /c "dotnet run --project src/VibeTracker.Client/VibeTracker.Client.csproj --urls http://localhost:5286"

REM Wait for client to start
echo ⏳ Waiting for client to start...
timeout /t 5 /nobreak >nul

echo.
echo 🎉 Now Playing Vibes Application Started Successfully!
echo =================================================
echo 🌐 API Server: http://localhost:5250
echo 🎵 Web Client: http://localhost:5286
echo.
echo 📝 Instructions:
echo 1. Open your browser to http://localhost:5286
echo 2. Use the navigation to submit vibes and view the vibe history
echo 3. Close this window or press Ctrl+C to stop monitoring
echo.
echo 🔍 API Endpoints:
echo - GET  http://localhost:5250/vibes (get all vibes)
echo - POST http://localhost:5250/vibes (submit a new vibe)
echo.
echo Two new windows have opened for the server and client.
echo You can close those windows to stop the applications.
echo.

pause