using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A single geocoded waypoint.
/// </summary>
public class DirectionsGeocodedWaypoint
{
    /// <summary>
    /// Whether the geocoder did not return an exact match for the original waypoint, though it was able to match part of the requested address.
    /// </summary>
    public bool? PartialMatch { get; set; }

    /// <summary>
    /// The place ID associated with the waypoint. 
    /// Place IDs uniquely identify a place in the Google Places database and on Google Maps. 
    /// Learn more about Place IDs in the Places API developer guide.
    /// </summary>
    public string PlaceId { get; set; }

    /// <summary>
    /// An array of strings denoting the type of the returned geocoded element. 
    /// For a list of possible strings, refer to the Address Component Types section of the Developer's Guide.
    /// </summary>
    public List<string> Types { get; set; }
}