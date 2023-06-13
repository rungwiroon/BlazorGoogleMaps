namespace GoogleMapsComponents.Maps;

/// <summary>
/// Options for the rendering of the map type control.
/// </summary>
public class MapTypeControlOptions
{
    /// <summary>
    /// IDs of map types to show in the control.
    /// </summary>
    public MapTypeId[] mapTypeIds { get; set; }

    public ControlPosition position { get; set; }

    public MapTypeControlStyle style { get; set; }
}