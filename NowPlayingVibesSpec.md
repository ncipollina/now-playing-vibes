# 🎧 Now Playing Vibes App – Full Stack .NET 9 Project

## 📝 Summary

Build a full-stack music-themed app called **"Now Playing Vibes"** using the following stack:

- **Frontend**: Blazor WebAssembly
- **Backend**: .NET 9 Minimal API using FastEndpoints
- **Data Access**: Dapper
- **Database**: SQLite (file-based, with a manual table creation script)
- **IDE**: JetBrains Rider

Users will submit a "vibe"—a mood category and a short message about what they’re listening to or doing. The app will save and display all submitted vibes.

---

## 🌐 App Features

### 1. Submit Vibe Page
- Dropdown to select a vibe (e.g., "Lo-fi", "Metal", "Synthwave")
- Textbox for current activity or music
- Submit button (POST to backend)

### 2. View Vibes Page
- Displays a list of all previously submitted vibes
- Includes: vibe type, message, and timestamp
- Styled as cards or table

---

## 🔧 Backend API Design (FastEndpoints)

### POST `/vibes`
- Accepts a `VibeRequest`
```csharp
public class VibeRequest
{
    public string VibeType { get; set; }
    public string Message { get; set; }
}
```

- Inserts into SQLite using Dapper

### GET `/vibes`
- Returns all vibe entries:
```csharp
public class VibeEntry
{
    public int Id { get; set; }
    public string VibeType { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
```

---

## 🗃️ Database

Use SQLite with Dapper and a table creation script that runs on startup:

```sql
CREATE TABLE IF NOT EXISTS Vibes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    VibeType TEXT NOT NULL,
    Message TEXT NOT NULL,
    Timestamp TEXT NOT NULL
);
```

---

## 🗂️ Project Structure

- `VibeTracker.sln`
  - `src/`
     - `VibeTracker.Server/` – Minimal API + FastEndpoints + Dapper
     - `VibeTracker.Client/` – Blazor WebAssembly frontend
     - `VibeTracker.Shared/` – Shared DTOs (optional)

---

## ✅ Task List

### SETUP
- [x] Create solution with 3 projects: Server, Client, Shared
- [x] Enable CORS in backend for Blazor client

### DATABASE
- [x] Add SQLite NuGet package: `Microsoft.Data.Sqlite`
- [x] On app startup, run `CREATE TABLE IF NOT EXISTS` via Dapper

### BACKEND (Server)
- [x] Create DTOs: `VibeRequest`, `VibeEntry`
- [x] Create `IVibeRepository` and `VibeRepository` (using Dapper)
- [x] Add POST endpoint to insert vibe into DB
- [x] Add GET endpoint to retrieve all vibes
- [x] Register Dapper + repo + endpoints in `Program.cs`

### FRONTEND (Client)
- [x] Add dropdown with predefined vibes
- [x] Create `SubmitVibe.razor` with form and HTTP POST to API
- [x] Create `ViewVibes.razor` that fetches and displays entries
- [x] Update `NavMenu.razor` to link both pages

### OPTIONAL/EXTRA
- [x] Add emoji icons next to vibes
- [x] Use random pastel background per card
- [x] Add toast notifications or form validation
- [x] Display relative time (e.g., "2 minutes ago")

### BONUS FEATURES ADDED
- [x] Enhanced UX with View Vibes as home page
- [x] Responsive Bootstrap styling with beautiful gradients
- [x] Form validation with loading states
- [x] Auto-navigation after successful submission
- [x] Comprehensive error handling
- [x] Dismissible toast notifications with icons
- [x] PowerShell and batch startup scripts
- [x] Comprehensive documentation and README files

---

## 🎯 Goals

By the end of the session, you should be able to:
- Submit a vibe entry from the UI
- See it appear immediately on the Vibe History page
- Run entirely locally with Rider using SQLite file-based DB
