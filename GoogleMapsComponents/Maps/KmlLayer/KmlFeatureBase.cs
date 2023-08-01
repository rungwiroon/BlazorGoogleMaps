namespace GoogleMapsComponents.Maps.KmlLayer;

public abstract class KmlFeatureBase
{
    /// <summary>
    /// The feature's &lt;atom:author>, extracted from the layer markup (if specified).
    /// </summary>
    public KmlAuthor? Author { get; set; }
    /// <summary>
    /// The feature's &lt;description>, extracted from the layer markup.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// The feature's &lt;name>, extracted from the layer markup.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// The feature's &lt;Snippet>, extracted from the layer markup.
    /// </summary>
    public string? Snippet { get; set; }
}