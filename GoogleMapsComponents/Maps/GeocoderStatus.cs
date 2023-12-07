using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The status returned by the Geocoder on the completion of a call to geocode(). Specify these by value, or by using the constant's name.
/// For example, 'OK' or google.maps.GeocoderStatus.OK.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GeocoderStatus
{
    /// <summary>
    /// There was a problem contacting the Google servers.
    /// </summary>
    [EnumMember(Value = "error")]
    Error,

    /// <summary>
    /// The <see cref="GeocoderRequest"></see> was invalid.
    /// </summary>
    [EnumMember(Value = "invalid_request")]
    InvalidRequest,

    /// <summary>
    /// The response contains a valid <see cref="GeocoderResponse"></see>.
    /// </summary>
    [EnumMember(Value = "ok")]
    Ok,

    /// <summary>
    /// The webpage has gone over the requests limit in too short a period of time.
    /// </summary>
    [EnumMember(Value = "over_query_limit")]
    OverQueryLimit,

    /// <summary>
    /// The webpage is not allowed to use the <see cref="Geocoder"></see>.
    /// </summary>
    [EnumMember(Value = "request_denied")]
    RequestDenied,

    /// <summary>
    /// A geocoding request could not be processed due to a server error.
    /// The request may succeed if you try again.
    /// </summary>
    [EnumMember(Value = "unknown_error")]
    UnknownError,

    /// <summary>
    /// No result was found for this <see cref="GeocoderRequest"></see>.
    /// </summary>
    [EnumMember(Value = "zero_results")]
    ZeroResults
}