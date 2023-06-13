namespace GoogleMapsComponents.Maps;

/// <summary>
/// Possible values for vehicle types. These values are specifed as strings, i.e. 'BUS' or 'TRAIN'.
/// </summary>
public enum VehicleType
{
    /// <summary>
    /// Bus.
    /// </summary>
    Bus,

    /// <summary>
    /// A vehicle that operates on a cable, usually on the ground. Aerial cable cars may be of the type GONDOLA_LIFT.
    /// </summary>
    CableCar,

    /// <summary>
    /// Commuter rail.
    /// </summary>
    CommuterTrain,

    /// <summary>
    /// Ferry.
    /// </summary>
    Ferry,

    /// <summary>
    /// A vehicle that is pulled up a steep incline by a cable.
    /// </summary>
    Funicular,

    /// <summary>
    /// An aerial cable car.
    /// </summary>
    GondolaLift,

    /// <summary>
    /// Heavy rail.
    /// </summary>
    HeavyRail,

    /// <summary>
    /// High speed train.
    /// </summary>
    HighSpeedTrain,

    /// <summary>
    /// Intercity bus.
    /// </summary>
    IntercityBus,

    /// <summary>
    /// Light rail.
    /// </summary>
    MetroRail,

    /// <summary>
    /// Monorail.
    /// </summary>
    MonoRail,

    /// <summary>
    /// Other vehicles.
    /// </summary>
    Other,

    /// <summary>
    /// Rail.
    /// </summary>
    Rail,

    /// <summary>
    /// Share taxi is a sort of bus transport with ability to drop off and pick up passengers anywhere on its route. Generally share taxi uses minibus vehicles.
    /// </summary>
    ShareTaxi,

    /// <summary>
    /// Underground light rail.
    /// </summary>
    Subway,

    /// <summary>
    /// Above ground light rail.
    /// </summary>
    Tram,

    /// <summary>
    /// Trolleybus.
    /// </summary>
    TrolleyBus
}