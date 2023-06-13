using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Information about the transit line that operates this transit step.
/// </summary>
public class TransitLine
{
    /// <summary>
    /// The transit agency that operates this transit line.
    /// </summary>
    public IEnumerable<TransitAgency> Agencies { get; set; }

    /// <summary>
    /// The color commonly used in signage for this transit line, represented as a hex string.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// The URL for an icon associated with this line.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// The full name of this transit line, e.g. "8 Avenue Local".
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The short name of this transit line, e.g. "E".
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// The text color commonly used in signage for this transit line, represented as a hex string.
    /// </summary>
    public string TextColor { get; set; }

    /// <summary>
    /// The agency's URL which is specific to this transit line.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The type of vehicle used, e.g. train or bus.
    /// </summary>
    public TransitVehicle Vehicle { get; set; }
}