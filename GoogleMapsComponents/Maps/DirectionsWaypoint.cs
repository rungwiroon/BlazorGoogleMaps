using OneOf;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A DirectionsWaypoint represents a location between origin and destination through which the trip should be routed.
/// </summary>
public class DirectionsWaypoint
{
    /// <summary>
    /// Waypoint location. Can be an address string, a LatLng, or a Place. 
    /// Optional.
    /// </summary>
    public OneOf<string, LatLngLiteral, Place> Location { get; set; }

    /// <summary>
    /// If true, indicates that this waypoint is a stop between the origin and destination. 
    /// This has the effect of splitting the route into two legs. 
    /// If false, indicates that the route should be biased to go through this waypoint, but not split into two legs. 
    /// This is useful if you want to create a route in response to the user dragging waypoints on a map. 
    /// This value is true by default. Optional.
    /// </summary>
    public bool Stopover { get; set; }
}