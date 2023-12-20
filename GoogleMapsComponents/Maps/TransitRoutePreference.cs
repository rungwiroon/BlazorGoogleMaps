namespace GoogleMapsComponents.Maps;

/// <summary>
/// The valid transit route type that can be specified in a TransitOptions.
/// </summary>
public enum TransitRoutePreference
{
    /// <summary>
    /// Specifies that the calculated route should prefer a limited number of transfers.
    /// </summary>
    FewerTransfers,

    /// <summary>
    /// Specifies that the calculated route should prefer limited amounts of walking.
    /// </summary>
    LessWalking
}