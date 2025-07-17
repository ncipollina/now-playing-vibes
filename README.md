# 🎵 Now Playing Vibes

A full-stack .NET 9 Blazor WebAssembly application for sharing music vibes with your team. Built with modern web technologies and a focus on user experience.

## ✨ Features

- **🎵 Submit Vibes**: Choose from 10 music genres and share what you're listening to
- **📱 Responsive Design**: Beautiful, mobile-friendly interface with Bootstrap
- **🎨 Beautiful UI**: Each vibe card has a unique pastel gradient background
- **⏰ Real-time Updates**: See relative timestamps ("2 minutes ago") and refresh in real-time
- **🔔 Toast Notifications**: Dismissible alerts with icons for better UX
- **💾 Local Storage**: SQLite database with automatic table creation
- **🚀 Easy Setup**: One-click startup scripts for Windows and cross-platform

## 🛠️ Tech Stack

- **Frontend**: Blazor WebAssembly (.NET 9)
- **Backend**: ASP.NET Core Minimal API with FastEndpoints
- **Database**: SQLite with Dapper ORM
- **Styling**: Bootstrap 5 with custom gradients
- **Build System**: .NET CLI and MSBuild

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK or higher
- PowerShell (for the startup script)

### Option 1: PowerShell Script (Recommended)
```powershell
# Clone the repository
git clone https://github.com/ncipollina/now-playing-vibes.git
cd now-playing-vibes

# Run the startup script
./start-vibes-app.ps1
```

### Option 2: Windows Batch Script
```cmd
# Clone and run
git clone https://github.com/ncipollina/now-playing-vibes.git
cd now-playing-vibes
start-vibes-app.bat
```

### Option 3: Manual Setup
```bash
# Clone the repository
git clone https://github.com/ncipollina/now-playing-vibes.git
cd now-playing-vibes

# Build the solution
dotnet build

# Terminal 1 - Start the API server
dotnet run --project src/VibeTracker.Server --urls http://localhost:5250

# Terminal 2 - Start the Blazor client
dotnet run --project src/VibeTracker.Client --urls http://localhost:5286
```

## 🌐 Usage

1. **Open your browser** to `http://localhost:5286`
2. **Submit a vibe** using the form with genre selection and message
3. **View all vibes** on the home page with beautiful cards
4. **Enjoy the experience** with real-time updates and smooth animations

## 📁 Project Structure

```
now-playing-vibes/
├── src/
│   ├── VibeTracker.Server/     # FastEndpoints API with SQLite
│   ├── VibeTracker.Client/     # Blazor WebAssembly frontend
│   └── VibeTracker.Shared/     # Shared DTOs and models
├── start-vibes-app.ps1         # PowerShell startup script
├── start-vibes-app.bat         # Windows batch startup script
├── README-STARTUP.md           # Detailed startup instructions
└── CLAUDE.md                   # Development guidance
```

## 🎯 API Endpoints

- **GET** `/vibes` - Retrieve all submitted vibes
- **POST** `/vibes` - Submit a new vibe

### Example Request
```json
{
  "vibeType": "Lo-fi",
  "message": "Working on some code with chill beats"
}
```

### Example Response
```json
{
  "id": 1,
  "vibeType": "Lo-fi",
  "message": "Working on some code with chill beats",
  "timestamp": "2025-07-17T21:30:00Z"
}
```

## 🎨 Available Vibe Types

| Genre | Emoji | Description |
|-------|-------|-------------|
| Lo-fi | 🎵 | Chill study beats |
| Metal | 🤘 | Heavy metal and rock |
| Synthwave | 🌆 | Retro electronic |
| Classical | 🎼 | Orchestral and instrumental |
| Jazz | 🎷 | Smooth jazz and blues |
| Rock | 🎸 | Alternative and indie rock |
| Electronic | 🎛️ | EDM and dance music |
| Indie | 🎤 | Independent artists |
| Hip-Hop | 🎤 | Rap and hip-hop |
| Folk | 🪕 | Acoustic and folk |

## 🔧 Development Features

- **Auto-reload**: Both server and client support hot reload
- **Error handling**: Comprehensive error messages and logging
- **Cross-platform**: Works on Windows, macOS, and Linux
- **Port management**: Automatic port conflict resolution
- **Database migrations**: Automatic SQLite table creation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 🙏 Acknowledgments

- Built with [.NET 9](https://dotnet.microsoft.com/)
- UI components from [Bootstrap 5](https://getbootstrap.com/)
- Icons from [Bootstrap Icons](https://icons.getbootstrap.com/)
- API framework: [FastEndpoints](https://fast-endpoints.com/)
- Data access: [Dapper](https://dapperlib.github.io/Dapper/)

---

**🎵 Start sharing your vibes today!** Perfect for teams who want to share their music taste and create a fun, collaborative atmosphere.