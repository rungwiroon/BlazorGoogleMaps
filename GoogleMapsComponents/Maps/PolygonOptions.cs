using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// PolygonOptions object used to define the properties that can be set on a Polygon.
/// </summary>
public class PolygonOptions : ListableEntityOptionsBase
{
    /// <summary>
    /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.<br/>
    /// Defaults to false.
    /// </summary>
    public bool? Editable { get; set; }

    /// <summary>
    /// The fill color. All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string FillColor { get; set; }

    /// <summary>
    /// The fill opacity between 0.0 and 1.0
    /// </summary>
    public float? FillOpacity { get; set; }

    /// <summary>
    /// When true, edges of the polygon are interpreted as geodesic and will follow the curvature of the Earth.<br/>
    /// When false, edges of the polygon are rendered as straight lines in screen space.<br/>
    /// Note that the shape of a geodesic polygon may appear to change when dragged, as the dimensions are maintained relative to the surface of the earth.<br/>
    /// Defaults to false.
    /// </summary>
    public bool? Geodesic { get; set; }

    /// <summary>
    /// The ordered sequence of coordinates that designates a closed loop.<br/>
    /// Unlike polylines, a polygon may consist of one or more paths.<br/>
    /// As a result, the paths property may specify one or more arrays of LatLng coordinates.<br/>
    /// Paths are closed automatically; do not repeat the first vertex of the path as the last vertex.<br/>
    /// Simple polygons may be defined using a single array of LatLngs. More complex polygons may specify an array of arrays.<br/>
    /// Any simple arrays are converted into MVCArrays.<br/>
    /// Inserting or removing LatLngs from the MVCArray will automatically update the polygon on the map.
    /// </summary>
    public IEnumerable<IEnumerable<LatLngLiteral>> Paths { get; set; }

    /// <summary>
    /// The stroke color. All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string StrokeColor { get; set; }

    /// <summary>
    /// The stroke opacity between 0.0 and 1.0
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The stroke position. Defaults to CENTER. This property is not supported on Internet Explorer 8 and earlier.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<StrokePosition>))]
    public StrokePosition StrokePosition { get; set; }

    /// <summary>
    /// The stroke width in pixels.
    /// </summary>
    public int? StrokeWeight { get; set; }
}