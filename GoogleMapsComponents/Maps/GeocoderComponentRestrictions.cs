namespace GoogleMapsComponents.Maps;

/// <summary>
/// GeocoderComponentRestrictions represents a set of filters that resolve to a specific area.
/// For details on how this works, see:
/// https://developers.google.com/maps/documentation/javascript/geocoding#ComponentFiltering
/// </summary>
public class GeocoderComponentRestrictions
{
    /// <summary>
    /// (optional) Matches all the administrative_area levels.
    /// </summary>
    public string? AdministrativeArea { get; set; }

    /// <summary>
    /// Matches a country name or a two letter ISO 3166-1 country code.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Matches against both locality and sublocality types.
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Matches postal_code and postal_code_prefix.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Matches the long or short name of a route.
    /// </summary>
    public string? Route { get; set; }
}