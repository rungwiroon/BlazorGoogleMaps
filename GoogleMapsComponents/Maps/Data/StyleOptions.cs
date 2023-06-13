using OneOf;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// These options specify the way a Feature should appear when displayed on a map.
/// </summary>
public class StyleOptions
{
    /// <summary>
    /// If true, the marker receives mouse and touch events. Default value is true.
    /// </summary>
    public bool? Clickable { get; set; }

    /// <summary>
    /// Mouse cursor to show on hover. Only applies to point geometries.
    /// </summary>
    public string Cursor { get; set; }

    /// <summary>
    /// If true, the object can be dragged across the map and the underlying feature will have its geometry updated.
    /// Default value is false.
    /// </summary>
    public bool? Draggable { get; set; }

    /// <summary>
    /// If true, the object can be edited by dragging control points and the underlying feature will have its geometry updated.
    /// Only applies to LineString and Polygon geometries. Default value is false.
    /// </summary>
    public bool? Editable { get; set; }

    /// <summary>
    /// The fill color.
    /// All CSS3 colors are supported except for extended named colors.
    /// Only applies to polygon geometries.
    /// </summary>
    public string FillColor { get; set; }

    /// <summary>
    /// The fill opacity between 0.0 and 1.0. Only applies to polygon geometries.
    /// </summary>
    public float FillOpacity { get; set; }

    /// <summary>
    /// Icon for the foreground.
    /// If a string is provided, it is treated as though it were an Icon with the string as url.
    /// Only applies to point geometries.
    /// </summary>
    public OneOf<string, Icon, Symbol>? Icon { get; set; }

    /// <summary>
    /// Defines the image map used for hit detection.
    /// Only applies to point geometries.
    /// </summary>
    public MarkerShape Shape { get; set; }

    /// <summary>
    /// The stroke color.
    /// All CSS3 colors are supported except for extended named colors.
    /// Only applies to line and polygon geometries.
    /// </summary>
    public string StrokeColor { get; set; }

    /// <summary>
    /// The stroke opacity between 0.0 and 1.0.
    /// Only applies to line and polygon geometries.
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The stroke width in pixels.
    /// Only applies to line and polygon geometries.
    /// </summary>
    public float? StrokeWeight { get; set; }

    /// <summary>
    /// Rollover text.
    /// Only applies to point geometries.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Whether the feature is visible.
    /// Defaults to true.
    /// </summary>
    public bool? Visible { get; set; }

    /// <summary>
    /// All features are displayed on the map in order of their zIndex, with higher values displaying in front of features with lower values.
    /// Markers are always displayed in front of line-strings and polygons.
    /// </summary>
    public int ZIndex { get; set; }
}