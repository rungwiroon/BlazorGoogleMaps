using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ClientSideDemo;

public class Startup
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        var apiKey = builder.Configuration["GoogleMaps:ApiKey"] ?? throw new InvalidOperationException("Google Maps API key not configured");

        var apiOptions = new MapApiLoadOptions(apiKey)
        {
            Version = "weekly",
            MapId = Guid.NewGuid().ToString()
        };
        builder.Services.AddSingleton(apiOptions);
        builder.Services.AddBlazorGoogleMaps(apiOptions);

        builder.RootComponents.Add<App>("app");
        await builder.Build().RunAsync();
    }
}
