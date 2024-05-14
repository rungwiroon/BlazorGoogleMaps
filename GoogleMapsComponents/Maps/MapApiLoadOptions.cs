namespace GoogleMapsComponents.Maps;

/// <summary>
/// All options
/// https://developers.google.com/maps/documentation/javascript/load-maps-js-api#required_parameters <br/>
/// </summary>
public class MapApiLoadOptions(string key)
{
    /// <summary>
    /// Default is weekly.
    /// https://developers.google.com/maps/documentation/javascript/versions
    /// </summary>
    public string Version { get; set; } = "weekly";

    /// <summary>
    /// Comma separated list of libraries to load.
    /// https://developers.google.com/maps/documentation/javascript/libraries
    /// </summary>
    public string? Libraries { get; set; } = "places,visualization,drawing,marker";

    /// <summary>
    /// Synchronous key provider function used by the default implementation
    /// </summary>
    public string? Key { get; set; } = key;
}