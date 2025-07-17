using FastEndpoints;
using VibeTracker.Server.Data;
using VibeTracker.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddFastEndpoints();

// Add session support for Spotify tokens
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure SQLite connection string
var connectionString = "Data Source=vibes.db";
builder.Services.AddSingleton<IVibeRepository>(provider => new VibeRepository(connectionString));
builder.Services.AddSingleton(provider => new DatabaseInitializer(connectionString));

// Add Spotify service
builder.Services.AddSingleton<ISpotifyService, SpotifyService>();

// Add CORS for Blazor client
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize database
var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.InitializeAsync();

// Configure middleware
app.UseCors();
app.UseSession();
app.UseFastEndpoints();

app.Run();
