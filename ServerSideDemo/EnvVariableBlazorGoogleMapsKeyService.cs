using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System;
using System.Threading.Tasks;

namespace ServerSideDemo;

public class EnvVariableBlazorGoogleMapsKeyService : IBlazorGoogleMapsKeyService
{
    public Task<MapApiLoadOptions> GetApiOptions()
    {

        var key = Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY");
        if (string.IsNullOrEmpty(key))
        {
            key = "AIzaSyBdkgvniMdyFPAcTlcZivr8f30iU-kn1T0";
        }

        IsApiInitialized = true;

        return Task.FromResult(new MapApiLoadOptions(key)
        {
            Version = "beta",
            // Language = "ja"
        });
    }

    public bool IsApiInitialized { get; set; }
}