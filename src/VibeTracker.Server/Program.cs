using FastEndpoints;
using VibeTracker.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddFastEndpoints();

// Configure SQLite connection string
var connectionString = "Data Source=vibes.db";
builder.Services.AddSingleton<IVibeRepository>(provider => new VibeRepository(connectionString));
builder.Services.AddSingleton(provider => new DatabaseInitializer(connectionString));

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
app.UseFastEndpoints();

app.Run();
