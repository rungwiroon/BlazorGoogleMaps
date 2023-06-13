using GoogleMapsComponents.Maps.Places;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A single geocoder result retrieved from the geocode server.
/// A geocode request may return multiple result objects.
/// Note that though this result is "JSON-like," it is not strictly JSON, as it indirectly includes a LatLng object.
/// </summary>
public class GeocoderResult
{
    /// <summary>
    /// An array of <see cref="GeocoderAddressComponent"></see>s
    /// </summary>
    [JsonPropertyName("address_components")]
    public GeocoderAddressComponent[] AddressComponents { get; set; } = default!;

    /// <summary>
    /// A string containing the human-readable address of this location.
    /// </summary>
    [JsonPropertyName("formatted_address")]
    public string FormattedAddress { get; set; } = default!;

    /// <summary>
    /// A GeocoderGeometry object
    /// </summary>
    public GeocoderGeometry Geometry { get; set; } = default!;

    /// <summary>
    /// The place ID associated with the location.
    /// Place IDs uniquely identify a place in the Google Places database and on Google Maps.
    /// Learn more about Place IDs in the Places API developer guide.
    /// </summary>
    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; } = default!;

    /// <summary>
    /// An array of strings denoting the type of the returned geocoded element.
    /// For a list of possible strings, refer to the Address Component Types section of the Developer's Guide.
    /// </summary>
    public string[] Types { get; set; } = default!;

    /// <summary>
    /// (optional) Whether the geocoder did not return an exact match for the original request, though it was able to match part of the requested address.
    /// If an exact match, the value will be undefined.
    /// </summary>
    [JsonPropertyName("partial_match")]
    public bool? PartialMatch { get; set; }

    /// <summary>
    /// (optional) The plus code associated with the location.
    /// </summary>
    [JsonPropertyName("plus_code")]
    public PlacePlusCode? PlusCode { get; set; }

    /// <summary>
    /// (optional) An array of strings denoting all the localities contained in a postal code.
    /// This is only present when the result is a postal code that contains multiple localities.
    /// </summary>
    [JsonPropertyName("postcode_localities")]
    public string[]? PostcodeLocalities { get; set; }
}