using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The status returned by the Geocoder on the completion of a call to geocode(). Specify these by value, or by using the constant's name.
/// For example, 'OK' or google.maps.GeocoderStatus.OK.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GeocoderLocationType
{
    /// <summary>
    /// The returned result is approximate.
    /// </summary>
    [EnumMember(Value = "approximate")]
    Approximate,

    /// <summary>
    /// The returned result is the geometric center of a result such a line (e.g. street) or polygon (region).
    /// </summary>
    [EnumMember(Value = "geometric_center")]
    GeometricCenter,

    /// <summary>
    /// The returned result reflects an approximation (usually on a road) interpolated between two precise points (such as intersections).
    /// Interpolated results are generally returned when rooftop geocodes are unavailable for a street address.
    /// </summary>
    [EnumMember(Value = "range_interpolated")]
    RangeInterpolated,

    /// <summary>
    /// The returned result reflects a precise geocode.
    /// </summary>
    [EnumMember(Value = "rooftop")]
    Rooftop
}