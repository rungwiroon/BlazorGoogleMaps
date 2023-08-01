namespace GoogleMapsComponents.Maps.KmlLayer;

public class KmlFeatureData : KmlFeatureBase
{
    /// <summary>
    /// The feature's &lt;id&gt; extracted from the layer markup. If no &lt;id&gt; has been specified, a unique ID will be generated for this feature.
    /// </summary>
    public string Id { get; set; } = string.Empty; //Should always be present.
    /// <summary>
    /// The feature's balloon styled text, if set.
    /// </summary>
    public string? InfoWindowHtml { get; set; }
}