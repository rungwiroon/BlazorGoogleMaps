namespace GoogleMapsComponents.Maps;

/// <summary>
/// Details about the departure, arrival, and mode of transit used in this step.
/// </summary>
public class TransitDetails
{
    /// <summary>
    /// The arrival stop of this transit step.
    /// </summary>
    public TransitStop ArrivalStop { get; set; }

    /// <summary>
    /// The arrival time of this step, specified as a Time object.
    /// </summary>
    public Time ArrivalTime { get; set; }

    /// <summary>
    /// The departure stop of this transit step.
    /// </summary>
    public TransitStop DepartureStop { get; set; }

    /// <summary>
    /// The departure time of this step, specified as a Time object.
    /// </summary>
    public Time DepartureTime { get; set; }

    /// <summary>
    /// The direction in which to travel on this line, as it is marked on the vehicle or at the departure stop.
    /// </summary>
    public string Headsign { get; set; }

    /// <summary>
    /// The expected number of seconds between equivalent vehicles at this stop.
    /// </summary>
    public string Headway { get; set; }

    /// <summary>
    /// Details about the transit line used in this step.
    /// </summary>
    public TransitLine Line { get; set; }

    /// <summary>
    /// The number of stops on this step. Includes the arrival stop, but not the departure stop.
    /// </summary>
    public int NumStops { get; set; }
}