using GoogleMapsComponents.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// RectangleOptions object used to define the properties that can be set on a Rectangle.
/// </summary>
public class RectangleOptions
{
    /// <summary>
    /// The bounds.
    /// </summary>
    public LatLngBoundsLiteral? Bounds { get; set; }

    /// <summary>
    /// Indicates whether this Rectangle handles mouse events. Defaults to true.
    /// </summary>
    public bool? Clickable { get; set; }

    /// <summary>
    /// If set to true, the user can drag this rectangle over the map. Defaults to false.
    /// </summary>
    public bool? Draggable { get; set; }

    /// <summary>
    /// If set to true, the user can edit this rectangle by dragging the control points shown at the corners and on each edge. 
    /// Defaults to false.
    /// </summary>
    public bool? Editable { get; set; }

    /// <summary>
    /// The fill color. All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string? FillColor { get; set; }

    /// <summary>
    /// The fill opacity between 0.0 and 1.0
    /// </summary>
    public float FillOpacity { get; set; }

    /// <summary>
    /// Map on which to display Rectangle.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map Map { get; set; }

    /// <summary>
    /// The stroke color. All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string? StrokeColor { get; set; }

    /// <summary>
    /// The stroke opacity between 0.0 and 1.0
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The stroke position. 
    /// Defaults to CENTER. 
    /// This property is not supported on Internet Explorer 8 and earlier.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<StrokePosition>))]
    public StrokePosition? StrokePosition { get; set; }

    /// <summary>
    /// The stroke width in pixels.
    /// </summary>
    public int? StrokeWeight { get; set; }

    /// <summary>
    /// Whether this rectangle is visible on the map. Defaults to true.
    /// </summary>
    public bool? Visible { get; set; }

    /// <summary>
    /// The zIndex compared to other polys.
    /// </summary>
    public int ZIndex { get; set; }
}