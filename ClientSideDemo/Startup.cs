using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ClientSideDemo;

public class Startup
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        var apiOptions = new MapApiLoadOptions("AIzaSyBdkgvniMdyFPAcTlcZivr8f30iU-kn1T0")
        {
            Version = "weekly"
        };
        builder.Services.AddSingleton(apiOptions);
        builder.Services.AddBlazorGoogleMaps(apiOptions);

        builder.RootComponents.Add<App>("app");
        await builder.Build().RunAsync();
    }
}