using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A single address component within a <see cref="GeocoderResult"></see>. A full address may consist of multiple address components.
/// </summary>
public class GeocoderAddressComponent
{
    /// <summary>
    /// The full text.
    /// </summary>
    [JsonPropertyName("long_name")]
    public string LongName { get; set; } = default!;

    /// <summary>
    /// The abbreviated, short text.
    /// </summary>
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; } = default!;

    /// <summary>
    /// An array of strings denoting the type.
    /// A list of valid types can be found at:
    /// https://developers.google.com/maps/documentation/javascript/geocoding#GeocodingAddressTypes
    /// </summary>
    public string[] Types { get; set; } = default!;
}