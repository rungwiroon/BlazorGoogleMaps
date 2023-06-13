namespace GoogleMapsComponents.Maps.Visualization;

/// <summary>
/// A data point entry for a heatmap. This is a geographical data point with a weight attribute.
/// </summary>
public class WeightedLocation
{
    /// <summary>
    /// The location of the data point.
    /// </summary>
    public LatLngLiteral Location { get; set; }

    /// <summary>
    /// The weighting value of the data point.
    /// </summary>
    public float Weight { get; set; }
}