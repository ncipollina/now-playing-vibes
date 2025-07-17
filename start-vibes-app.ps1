#!/usr/bin/env pwsh

# Now Playing Vibes Application Startup Script
# This script starts both the server and client applications with proper port management

Write-Host "🎵 Starting Now Playing Vibes Application..." -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan

# Function to kill processes on specific ports
function Stop-ProcessOnPort {
    param (
        [int]$Port,
        [string]$Description
    )
    
    Write-Host "🔍 Checking for processes on port $Port ($Description)..." -ForegroundColor Yellow
    
    # Find and kill processes on the specified port
    if ($IsWindows) {
        $processes = netstat -ano | Select-String ":$Port " | ForEach-Object { ($_ -split '\s+')[-1] }
        foreach ($pid in $processes) {
            if ($pid -and $pid -ne "0") {
                try {
                    Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
                    Write-Host "✅ Killed process $pid on port $Port" -ForegroundColor Green
                } catch {
                    # Process might already be gone, ignore
                }
            }
        }
    } else {
        # macOS/Linux
        $processes = lsof -ti:$Port 2>/dev/null
        if ($processes) {
            foreach ($processId in $processes) {
                try {
                    kill -9 $processId 2>/dev/null
                    Write-Host "✅ Killed process $processId on port $Port" -ForegroundColor Green
                } catch {
                    # Process might already be gone, ignore
                }
            }
        }
    }
}

# Function to wait for a service to be ready
function Wait-ForService {
    param (
        [string]$Url,
        [int]$TimeoutSeconds = 30
    )
    
    $timeout = [DateTime]::Now.AddSeconds($TimeoutSeconds)
    
    while ([DateTime]::Now -lt $timeout) {
        try {
            # Skip SSL certificate validation for development
            $response = Invoke-WebRequest -Uri $Url -Method GET -TimeoutSec 5 -SkipCertificateCheck -ErrorAction Stop
            return $true
        } catch {
            Start-Sleep -Seconds 1
        }
    }
    return $false
}

# Check if we're in the correct directory
if (-not (Test-Path "VibeTracker.sln")) {
    Write-Host "❌ Error: VibeTracker.sln not found. Please run this script from the project root directory." -ForegroundColor Red
    exit 1
}

# Kill any existing processes on the ports we'll use
Stop-ProcessOnPort -Port 3000 -Description "Server (HTTPS)"
Stop-ProcessOnPort -Port 5287 -Description "Client (HTTPS)"
Stop-ProcessOnPort -Port 8080 -Description "Server (old HTTPS)"
Stop-ProcessOnPort -Port 7001 -Description "Server (old HTTPS)"
Stop-ProcessOnPort -Port 5249 -Description "Server (default HTTP)"
Stop-ProcessOnPort -Port 5250 -Description "Server (custom HTTP)"
Stop-ProcessOnPort -Port 5285 -Description "Client (default HTTP)"
Stop-ProcessOnPort -Port 5286 -Description "Client (custom HTTP)"

# Kill any existing dotnet processes (more aggressive cleanup)
Write-Host "🧹 Cleaning up existing dotnet processes..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowTitle -like "*VibeTracker*" -or $_.ProcessName -eq "dotnet" } | Stop-Process -Force -ErrorAction SilentlyContinue

# Build the solution first
Write-Host "🔨 Building the solution..." -ForegroundColor Blue
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed! Please fix the build errors and try again." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green

# Start the server
Write-Host "🚀 Starting the API server..." -ForegroundColor Blue
$serverProcess = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/VibeTracker.Server/VibeTracker.Server.csproj", "--urls", "http://127.0.0.1:3000" -PassThru

# Wait a moment for server to start
Start-Sleep -Seconds 3

# Check if server is running
Write-Host "⏳ Waiting for server to be ready..." -ForegroundColor Yellow
if (Wait-ForService -Url "http://127.0.0.1:3000/vibes" -TimeoutSeconds 30) {
    Write-Host "✅ Server is ready at http://127.0.0.1:3000" -ForegroundColor Green
} else {
    Write-Host "❌ Server failed to start or is not responding" -ForegroundColor Red
    Write-Host "🔍 Check the server logs above for errors" -ForegroundColor Yellow
    exit 1
}

# Start the client
Write-Host "🚀 Starting the Blazor client..." -ForegroundColor Blue
$clientProcess = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/VibeTracker.Client/VibeTracker.Client.csproj", "--urls", "https://localhost:5287" -PassThru

# Wait a moment for client to start
Start-Sleep -Seconds 3

# Check if client is running
Write-Host "⏳ Waiting for client to be ready..." -ForegroundColor Yellow
if (Wait-ForService -Url "https://localhost:5287" -TimeoutSeconds 30) {
    Write-Host "✅ Client is ready at https://localhost:5287" -ForegroundColor Green
} else {
    Write-Host "❌ Client failed to start or is not responding" -ForegroundColor Red
    Write-Host "🔍 Check the client logs above for errors" -ForegroundColor Yellow
    # Don't exit here as server might still be useful
}

Write-Host ""
Write-Host "🎉 Now Playing Vibes Application Started Successfully!" -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Green
Write-Host "🌐 API Server: http://127.0.0.1:3000" -ForegroundColor Cyan
Write-Host "🎵 Web Client: https://localhost:5287" -ForegroundColor Cyan
Write-Host ""
Write-Host "📝 Instructions:" -ForegroundColor Yellow
Write-Host "1. Open your browser to https://localhost:5287" -ForegroundColor White
Write-Host "2. Use the navigation to submit vibes and view the vibe history" -ForegroundColor White
Write-Host "3. Press Ctrl+C to stop both applications" -ForegroundColor White
Write-Host ""
Write-Host "🔍 API Endpoints:" -ForegroundColor Yellow
Write-Host "- GET  http://127.0.0.1:3000/vibes (get all vibes)" -ForegroundColor White
Write-Host "- POST http://127.0.0.1:3000/vibes (submit a new vibe)" -ForegroundColor White
Write-Host ""

# Keep the script running and handle cleanup on exit
try {
    Write-Host "✋ Press Ctrl+C to stop the application..." -ForegroundColor Magenta
    
    # Wait for user to press Ctrl+C
    while ($true) {
        Start-Sleep -Seconds 1
    }
} finally {
    Write-Host ""
    Write-Host "🛑 Shutting down applications..." -ForegroundColor Yellow
    
    # Kill the processes we started
    if ($serverProcess -and !$serverProcess.HasExited) {
        $serverProcess.Kill()
        Write-Host "✅ Server stopped" -ForegroundColor Green
    }
    
    if ($clientProcess -and !$clientProcess.HasExited) {
        $clientProcess.Kill()
        Write-Host "✅ Client stopped" -ForegroundColor Green
    }
    
    # Clean up any remaining processes
    Stop-ProcessOnPort -Port 3000 -Description "Server cleanup"
    Stop-ProcessOnPort -Port 5287 -Description "Client cleanup"
    
    Write-Host "👋 Applications stopped. Thanks for using Now Playing Vibes!" -ForegroundColor Cyan
}