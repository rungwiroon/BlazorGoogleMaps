using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The directions response retrieved from the directions server. 
/// You can render these using a DirectionsRenderer or parse this object and render it yourself. 
/// You must display the warnings and copyrights as noted in the Google Maps Platform Terms of Service. 
/// Note that though this result is "JSON-like," it is not strictly JSON, as it indirectly includes LatLng objects.
/// </summary>
public class DirectionsResult
{
    /// <summary>
    /// An array of DirectionsGeocodedWaypoints, each of which contains information about the geocoding of origin, destination and waypoints.
    /// </summary>
    [JsonPropertyName("geocoded_waypoints")]
    public IEnumerable<DirectionsGeocodedWaypoint> GeoCodedWaypoints { get; set; }

    /// <summary>
    /// An array of DirectionsRoutes, each of which contains information about the legs and steps of which it is composed. 
    /// There will only be one route unless the DirectionsRequest was made with provideRouteAlternatives set to true.
    /// </summary>
    public IEnumerable<DirectionsRoute> Routes { get; set; }
}