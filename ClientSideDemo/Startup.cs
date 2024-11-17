using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace ClientSideDemo;

public class Startup
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddBlazorGoogleMaps(new MapApiLoadOptions("AIzaSyBdkgvniMdyFPAcTlcZivr8f30iU-kn1T0")
        {
            Version = "weekly"
        });

        builder.RootComponents.Add<App>("app");
        await builder.Build().RunAsync();
    }
}