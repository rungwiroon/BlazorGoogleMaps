using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// google.maps.places.PlaceResult interface
/// </summary>
/// <remarks>
/// This is currently incomplete. Some properties not yet mapped,
/// <see cref="https://developers-dot-devsite-v2-prod.appspot.com/maps/documentation/javascript/reference/places-service#PlaceResult"/>
/// for full list
/// </remarks>
public class PlaceResult
{
    /// <summary>
    /// The collection of address components for this Place’s location. Only available with PlacesService.getDetails.
    /// </summary>
    [JsonPropertyName("address_components")]
    public IEnumerable<GeocoderAddressComponent> AddressComponents { get; set; }

    /// <summary>
    /// The representation of the Place’s address in the adr microformat. Only available with PlacesService.getDetails.
    /// </summary>
    [JsonPropertyName("adr_address")]
    public string AdrAddress { get; set; }

    /// <summary>
    /// The Place’s geometry-related information.
    /// </summary>
    public PlaceGeometry Geometry { get; set; }

    /// <summary>
    /// The Place’s full address.
    /// </summary>
    [JsonPropertyName("formatted_address")]
    public string FormattedAddress { get; set; }

    /// <summary>
    /// URL to an image resource that can be used to represent this Place’s category.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// The Place’s name. Note: In the case of user entered Places, this is the raw text,
    /// as typed by the user. Please exercise caution when using this data, as malicious users
    /// may try to use it as a vector for code injection attacks
    /// (See http://en.wikipedia.org/wiki/Code_injection).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A unique identifier for the Place.
    /// </summary>
    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; }

    /// <summary>
    /// Plus codes can be used as a replacement for street addresses in places where they do not exist (where buildings are not numbered or streets are not named).
    /// The plus code is formatted as a global code and a compound code
    /// Typically, both the global code and compound code are returned. However, if the result is in a remote location (for example, an ocean or desert) only the global code may be returned.
    /// </summary>
    [JsonPropertyName("plus_code")]
    public PlacePlusCode PlusCode { get; set; }

    /// <summary>
    /// URL of the official Google page for this place. This is the Google-owned page that contains
    /// the best available information about the Place. Only available with PlacesService.getDetails.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The authoritative website for this Place, such as a business' homepage.
    /// Only available with PlacesService.getDetails.
    /// </summary>
    public string Website { get; set; }
}