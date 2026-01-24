using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System;
using System.Threading.Tasks;

namespace ServerSideDemo;

public class EnvVariableBlazorGoogleMapsKeyService : IBlazorGoogleMapsKeyService
{
    private readonly Action<MapApiLoadOptions>? _configureOptions;

    public EnvVariableBlazorGoogleMapsKeyService(Action<MapApiLoadOptions>? configureOptions = null)
    {
        _configureOptions = configureOptions;
    }

    public Task<MapApiLoadOptions> GetApiOptions()
    {

        var key = Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY");
        if (string.IsNullOrEmpty(key))
        {
            key = "GOOGLE_MAPS_API_KEY";
        }

        var mapId = Environment.GetEnvironmentVariable("GOOGLE_MAPS_MAP_ID");
        if (string.IsNullOrEmpty(mapId))
        {
            mapId = "MAP_ID";
        }

        //IsApiInitialized = true;

        var options = new MapApiLoadOptions(key)
        {
            Version = "beta",
            Libraries = "places,visualization,drawing,marker",
            MapId = mapId
            // Language = "ja"
        };
 
        _configureOptions?.Invoke(options);
        return Task.FromResult(options);
    }

    public bool IsApiInitialized { get; set; }
}
