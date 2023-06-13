namespace GoogleMapsComponents.Maps;

/// <summary>
/// The valid transit mode e.g. bus that can be specified in a TransitOptions. 
/// </summary>
public enum TransitMode
{
    /// <summary>
    /// Specifies bus as a preferred mode of transit.
    /// </summary>
    Bus,

    /// <summary>
    /// Specifies rail as a preferred mode of transit.
    /// </summary>
    Rail,

    /// <summary>
    /// Specifies subway as a preferred mode of transit.
    /// </summary>
    Subway,

    /// <summary>
    /// Specifies train as a preferred mode of transit.
    /// </summary>
    Train,

    /// <summary>
    /// Specifies tram as a preferred mode of transit.
    /// </summary>
    Tram
}