using OneOf;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A directions query to be sent to the DirectionsService.
/// </summary>
public class DirectionsRequest
{
    /// <summary>
    /// If true, instructs the Directions service to avoid ferries where possible. 
    /// Optional.
    /// </summary>
    public bool? AvoidFerries { get; set; }

    /// <summary>
    /// If true, instructs the Directions service to avoid highways where possible. 
    /// Optional.
    /// </summary>
    public bool? AvoidHighways { get; set; }

    /// <summary>
    /// If true, instructs the Directions service to avoid toll roads where possible. 
    /// Optional.
    /// </summary>
    public bool? AvoidTolls { get; set; }

    /// <summary>
    /// Location of destination. This can be specified as either a string to be geocoded, or a LatLng, or a Place. 
    /// Required.
    /// </summary>
    public OneOf<string, LatLngLiteral, Place> Destination { get; set; }

    /// <summary>
    /// Settings that apply only to requests where travelMode is DRIVING. 
    /// This object will have no effect for other travel modes.
    /// </summary>
    public DrivingOptions? DrivingOptions { get; set; }

    /// <summary>
    /// If set to true, the DirectionsService will attempt to re-order the supplied intermediate waypoints to minimize overall cost of the route. 
    /// If waypoints are optimized, inspect DirectionsRoute.waypoint_order in the response to determine the new ordering.
    /// </summary>
    public bool? OptimizeWaypoints { get; set; }

    /// <summary>
    /// Location of origin. 
    /// This can be specified as either a string to be geocoded, or a LatLng, or a Place. 
    /// Required.
    /// </summary>
    public OneOf<string, LatLngLiteral, Place> Origin { get; set; }

    /// <summary>
    /// Whether or not route alternatives should be provided. 
    /// Optional.
    /// </summary>
    public bool? ProvideRouteAlternatives { get; set; }

    /// <summary>
    /// Region code used as a bias for geocoding requests. 
    /// Optional.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Settings that apply only to requests where travelMode is TRANSIT. 
    /// This object will have no effect for other travel modes.
    /// </summary>
    public TransitOptions? TransitOptions { get; set; }

    /// <summary>
    /// Type of routing requested. 
    /// Required.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<TravelMode>))]
    public TravelMode TravelMode { get; set; }

    /// <summary>
    /// Preferred unit system to use when displaying distance. 
    /// Defaults to the unit system used in the country of origin.
    /// </summary>
    public UnitSystem? UnitSystem { get; set; }

    /// <summary>
    /// Array of intermediate waypoints. Directions are calculated from the origin to the destination by way of each waypoint in this array. 
    /// See the developer's guide for the maximum number of waypoints allowed. 
    /// Waypoints are not supported for transit directions. Optional.
    /// </summary>
    public IEnumerable<DirectionsWaypoint>? Waypoints { get; set; }
}