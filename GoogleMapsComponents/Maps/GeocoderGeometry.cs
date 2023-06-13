using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Geometry information about the <see cref="GeocoderResult"></see>
/// </summary>
public class GeocoderGeometry
{
    /// <summary>
    /// The latitude/longitude coordinates of this result
    /// </summary>
    public LatLngLiteral Location { get; set; } = default!;

    /// <summary>
    /// The type of location returned in <see cref="Location"></see>
    /// </summary>
    [JsonPropertyName("location_type")]
    [JsonConverter(typeof(EnumMemberConverter<GeocoderLocationType>))]
    public GeocoderLocationType LocationType { get; set; }

    /// <summary>
    /// The bounds of the recommended viewport for displaying the <see cref="GeocoderResult"></see>
    /// </summary>
    public LatLngBoundsLiteral Viewport { get; set; } = default!;

    /// <summary>
    /// (optional) The precise bounds of the <see cref="GeocoderResult"></see>, if applicable
    /// </summary>
    public LatLngBoundsLiteral? Bounds { get; set; }
}