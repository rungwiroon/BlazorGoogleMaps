using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A single geocoded waypoint.
/// </summary>
public class DirectionsRoute
{
    /// <summary>
    /// A single route containing a set of legs in a DirectionsResult. 
    /// Note that though this object is "JSON-like," it is not strictly JSON, as it directly and indirectly includes LatLng objects.
    /// </summary>
    public LatLngBoundsLiteral? Bounds { get; set; }

    /// <summary>
    /// Copyrights text to be displayed for this route.
    /// </summary>
    public string? Copyrights { get; set; }

    /// <summary>
    /// An array of DirectionsLegs, each of which contains information about the steps of which it is composed. 
    /// There will be one leg for each stopover waypoint or destination specified. 
    /// So a route with no stopover waypoints will contain one DirectionsLeg and a route with one stopover waypoint will contain two.
    /// </summary>
    public IEnumerable<DirectionsLeg> Legs { get; set; } = new List<DirectionsLeg>();

    /// <summary>
    /// An array of LatLngs representing the entire course of this route. 
    /// The path is simplified in order to make it suitable in contexts where a small number of vertices is required (such as Static Maps API URLs).
    /// </summary>
    [JsonPropertyName("overview_path")]
    public IEnumerable<LatLngLiteral> OverviewPath { get; set; } = new List<LatLngLiteral>();

    /// <summary>
    /// An encoded polyline representation of the route in overview_path. 
    /// This polyline is an approximate (smoothed) path of the resulting directions.
    /// </summary>
    [JsonPropertyName("overview_polyline")]
    public string? OverviewPolyline { get; set; }

    /// <summary>
    /// Contains a short textual description for the route, suitable for naming and disambiguating the route from alternatives.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Warnings to be displayed when showing these directions.
    /// </summary>
    public List<string> Warnings { get; set; } = new List<string>();

    /// <summary>
    /// If optimizeWaypoints was set to true, this field will contain the re-ordered permutation of the input waypoints. For example, if the input was:
    /// Origin: Los Angeles
    /// Waypoints: Dallas, Bangor, Phoenix
    /// Destination: New York
    ///  and the optimized output was ordered as follows:
    /// Origin: Los Angeles
    /// Waypoints: Phoenix, Dallas, Bangor
    /// Destination: New York
    ///  then this field will be an Array containing the values [2, 0, 1]. Note that the numbering of waypoints is zero-based.
    ///  If any of the input waypoints has stopover set to false, this field will be empty, since route optimization is not available for such queries.
    /// </summary>
    [JsonPropertyName("waypoint_order")]
    public List<int> WaypointOrder { get; set; } = new List<int>();

    /// <summary>
    /// The total fare for the whole transit trip. Only applicable to transit requests.
    /// </summary>
    public TransitFare? Fare { get; set; }
}