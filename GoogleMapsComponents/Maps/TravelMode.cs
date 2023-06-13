using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The valid travel modes that can be specified in a DirectionsRequest as well as the travel modes returned in a DirectionsStep. 
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TravelMode
{
    /// <summary>
    /// Specifies a bicycling directions request.
    /// </summary>
    [EnumMember(Value = "BICYCLING")]
    Bicycling,

    /// <summary>
    /// Specifies a driving directions request.
    /// </summary>
    [EnumMember(Value = "DRIVING")]
    Driving,

    /// <summary>
    /// Specifies a transit directions request.
    /// </summary>
    [EnumMember(Value = "TRANSIT")]
    Transit,

    /// <summary>
    /// Specifies a walking directions request.
    /// </summary>
    [EnumMember(Value = "WALKING")]
    Walking
}