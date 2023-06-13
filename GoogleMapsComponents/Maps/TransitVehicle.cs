namespace GoogleMapsComponents.Maps;

/// <summary>
/// Information about the vehicle that operates on a transit line.
/// </summary>
public class TransitVehicle
{
    /// <summary>
    /// A URL for an icon that corresponds to the type of vehicle used on this line.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// A URL for an icon that corresponds to the type of vehicle used in this region instead of the more general icon.
    /// </summary>
    public string LocalIcon { get; set; }

    /// <summary>
    /// A name for this type of TransitVehicle, e.g. "Train" or "Bus".
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The type of vehicle used, e.g. train, bus, or ferry.
    /// </summary>
    public VehicleType Type { get; set; }
}