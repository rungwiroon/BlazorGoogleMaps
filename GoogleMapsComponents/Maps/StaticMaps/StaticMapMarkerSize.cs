using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Sizes of the default Maps Static API marker icon.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#MarkerStyles
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<StaticMapMarkerSize>))]
public enum StaticMapMarkerSize
{
    /// <summary>
    /// The smallest marker. Labels are not rendered on tiny markers.
    /// </summary>
    [EnumMember(Value = "tiny")]
    Tiny,

    /// <summary>
    /// A mid sized marker. Labels are rendered.
    /// </summary>
    [EnumMember(Value = "mid")]
    Mid,

    /// <summary>
    /// A small marker. Labels are not rendered on small markers.
    /// </summary>
    [EnumMember(Value = "small")]
    Small
}
