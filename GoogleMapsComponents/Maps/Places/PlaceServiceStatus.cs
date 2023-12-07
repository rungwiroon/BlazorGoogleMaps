using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// The status returned by the PlacesService on the completion of its searches. Specify these by value, or by using the constant's name.
/// For example, 'OK' or google.maps.places.PlacesServiceStatus.OK.
/// Requires the &libraries=places URL parameter.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PlaceServiceStatus
{
    /// <summary>
    /// The request was invalid.
    /// </summary>
    [EnumMember(Value = "invalid_request")]
    InvalidRequest,

    /// <summary>
    /// The place referenced was not found.
    /// </summary>
    [EnumMember(Value = "not_found")]
    NotFound,

    /// <summary>
    /// The response contains a valid result.
    /// </summary>
    [EnumMember(Value = "ok")]
    Ok,

    /// <summary>
    /// The application has gone over its request quota.
    /// </summary>
    [EnumMember(Value = "over_query_limit")]
    OverQueryLimit,

    /// <summary>
    /// The application is not allowed to use the places service.
    /// </summary>
    [EnumMember(Value = "request_denied")]
    RequestDenied,

    /// <summary>
    /// The places request could not be processed due to a server error. 
    /// The request may succeed if you try again.
    /// </summary>
    [EnumMember(Value = "unknown_error")]
    UnknownError,

    /// <summary>
    /// No result was found for this request.
    /// </summary>
    [EnumMember(Value = "zero_results")]
    ZeroResults
}