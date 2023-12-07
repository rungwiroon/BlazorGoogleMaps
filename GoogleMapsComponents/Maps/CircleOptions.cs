using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// CircleOptions object used to define the properties that can be set on a Circle.
/// </summary>
public class CircleOptions : ListableEntityOptionsBase
{
    /// <summary>
    /// The center of the Circle.
    /// </summary>
    public LatLngLiteral Center { get; set; }

    /// <summary>
    /// If set to true, the user can edit this circle by dragging the control points shown at the center and around the circumference of the circle. 
    /// Defaults to false.
    /// </summary>
    public bool? Editable { get; set; }

    /// <summary>
    /// The fill color. 
    /// All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string FillColor { get; set; }

    /// <summary>
    /// The fill opacity between 0.0 and 1.0.
    /// </summary>
    public float? FillOpacity { get; set; }

    /// <summary>
    /// The radius in meters on the Earth's surface.
    /// </summary>
    public double? Radius { get; set; }

    /// <summary>
    /// The stroke color. 
    /// All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string StrokeColor { get; set; }

    /// <summary>
    /// The stroke opacity between 0.0 and 1.0.
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The stroke position. Defaults to CENTER. 
    /// This property is not supported on Internet Explorer 8 and earlier.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<StrokePosition>))]
    public StrokePosition? StrokePosition { get; set; }

    /// <summary>
    /// The stroke width in pixels.
    /// </summary>
    public int StrokeWeight { get; set; }
}