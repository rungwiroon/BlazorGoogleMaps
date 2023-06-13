using System;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Configures the DirectionsRequest when the travel mode is set to DRIVING.
/// </summary>
public class DrivingOptions
{
    /// <summary>
    /// The desired departure time for the route, specified as a Date object. 
    /// This must be specified for a DrivingOptions to be valid. 
    /// The departure time must be set to the current time or some time in the future. 
    /// It cannot be in the past.
    /// </summary>
    public DateTime DepartureTime { set; get; }

    /// <summary>
    /// The preferred assumption to use when predicting duration in traffic. 
    /// The default is BEST_GUESS.
    /// </summary>
    public TrafficModel TrafficModel { get; set; }
}