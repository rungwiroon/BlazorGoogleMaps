using OneOf;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The specification for a geocoding request to be sent to the <see cref="Geocoder"></see>.
/// </summary>
public class GeocoderRequest
{
    /// <summary>
    /// (optional) Address to geocode.
    /// One, and only one, of address, location and placeId must be supplied.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// (optional) LatLngBounds within which to search.
    /// </summary>
    public OneOf<LatLngBounds, LatLngBoundsLiteral>? Bounds { get; set; }

    /// <summary>
    /// (optional) Components are used to restrict results to a specific area.
    /// A filter consists of one or more of: route, locality, administrativeArea, postalCode, country.
    /// Only the results that match all the filters will be returned.
    /// Filter values support the same methods of spelling correction and partial matching as other geocoding requests.
    /// </summary>
    public GeocoderComponentRestrictions? ComponentRestrictions { get; set; }

    /// <summary>
    /// A language identifier for the language in which results should be returned, when possible.
    /// Find list of supported languages at:
    /// https://developers.google.com/maps/faq#languagesupport
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// (optional) LatLng (or LatLngLiteral) for which to search.
    /// The geocoder performs a reverse geocode. See Reverse Geocoding for more information.
    /// One, and only one, of address, location and placeId must be supplied.
    /// </summary>
    public LatLngLiteral? Location { get; set; }

    /// <summary>
    /// (optional) The place ID associated with the location. Place IDs uniquely identify a place in the Google Places database and on Google Maps.
    /// Learn more about place IDs in the Places API developer guide.
    /// The geocoder performs a reverse geocode. See Reverse Geocoding for more information.
    /// One, and only one, of address, location and placeId must be supplied.
    /// </summary>
    public string? PlaceId { get; set; }

    /// <summary>
    /// (optional) Country code used to bias the search, specified as a two-character (non-numeric) Unicode region subtag / CLDR identifier.
    /// See Google Maps Platform Coverage Details for supported regions.
    /// </summary>
    public string? Region { get; set; }
}