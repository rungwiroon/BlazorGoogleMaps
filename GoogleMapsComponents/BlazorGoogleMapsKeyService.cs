using System.Threading.Tasks;

namespace GoogleMapsComponents;

/// <summary>
/// Interface for the key service that is injected into the Map Component in order to retrieve
/// a google api key when injecting the google api JS package
/// </summary>
public interface IBlazorGoogleMapsKeyService
{
    public Task<Maps.MapApiLoadOptions> GetApiOptions();

    public bool IsApiInitialized { get; set; }
}

/// <summary>
/// Basic implmentation of <see cref="IBlazorGoogleMapsKeyService"/> to load Google API JS dynamically instead of through _Hosts.cshtml.<br/> 
/// Replace with your own service to provide keys via dynamic lookup.
/// </summary>
public class BlazorGoogleMapsKeyService : IBlazorGoogleMapsKeyService
{
    private readonly Maps.MapApiLoadOptions _initOptions;
    public bool IsApiInitialized { get; set; } = false;

    public BlazorGoogleMapsKeyService(string key)
    {
        _initOptions = new Maps.MapApiLoadOptions(key);
    }
    public BlazorGoogleMapsKeyService(Maps.MapApiLoadOptions opts)
    {
        _initOptions = opts;
    }
    public Task<Maps.MapApiLoadOptions> GetApiOptions()
    {
        // Can do async things to get the API key if needed here.
        return Task.FromResult(_initOptions);
    }
}
