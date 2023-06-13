using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The status returned by the DirectionsService on the completion of a call to route(). 
/// Specify these by value, or by using the constant's name. For example, 'OK' or google.maps.DirectionsStatus.OK.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DirectionsStatus
{
    /// <summary>
    /// The DirectionsRequest provided was invalid.
    /// </summary>
    [EnumMember(Value = "invalid_request")]
    InvalidRequest,

    /// <summary>
    /// Too many DirectionsWaypoints were provided in the DirectionsRequest. 
    /// See the developer's guide for the maximum number of waypoints allowed.
    /// </summary>
    [EnumMember(Value = "max_waypoint_exceeded")]
    MaxWaypointsExceeded,

    /// <summary>
    /// At least one of the origin, destination, or waypoints could not be geocoded.
    /// </summary>
    [EnumMember(Value = "not_found")]
    NotFound,

    /// <summary>
    /// The response contains a valid DirectionsResult.
    /// </summary>
    [EnumMember(Value = "ok")]
    Ok,

    /// <summary>
    /// The webpage has gone over the requests limit in too short a period of time.
    /// </summary>
    [EnumMember(Value = "over_query_limit")]
    OverQueryLimit,

    /// <summary>
    /// The webpage is not allowed to use the directions service.
    /// </summary>
    [EnumMember(Value = "request_denied")]
    RequestDenied,

    /// <summary>
    /// A directions request could not be processed due to a server error. 
    /// The request may succeed if you try again.
    /// </summary>
    [EnumMember(Value = "unknown_error")]
    UnknownError,

    /// <summary>
    /// No route could be found between the origin and destination.
    /// </summary>
    [EnumMember(Value = "zero_results")]
    ZeroResults
}