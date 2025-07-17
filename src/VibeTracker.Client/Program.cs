using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VibeTracker.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to connect to the API server
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5250") });

await builder.Build().RunAsync();
