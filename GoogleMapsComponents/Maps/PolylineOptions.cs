using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// PolylineOptions object used to define the properties that can be set on a Polyline.
/// </summary>
public class PolylineOptions : ListableEntityOptionsBase
{
    /// <summary>
    /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.<br/>
    /// Defaults to false.
    /// </summary>
    public bool? Editable { get; set; }

    /// <summary>
    /// When true, edges of the polygon are interpreted as geodesic and will follow the curvature of the Earth. When false, edges of the polygon are rendered as straight lines in screen space.<br/>
    /// Note that the shape of a geodesic polygon may appear to change when dragged, as the dimensions are maintained relative to the surface of the earth.<br/>
    /// Defaults to false.
    /// </summary>
    public bool? Geodesic { get; set; }

    /// <summary>
    /// The icons to be rendered along the polyline.
    /// </summary>
    public IEnumerable<IconSequence> Icons { get; set; }

    /// <summary>
    /// The ordered sequence of coordinates of the Polyline.
    /// </summary>
    public IEnumerable<LatLngLiteral> Path { get; set; }

    /// <summary>
    /// The stroke color. All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public string StrokeColor { get; set; }

    /// <summary>
    /// The stroke opacity between 0.0 and 1.0.
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The stroke width in pixels.
    /// </summary>
    public int? StrokeWeight { get; set; }

    /// <summary>
    /// Hide the undo button
    /// Undocumented property.
    /// https://issuetracker.google.com/issues/35821607
    /// </summary>
    public bool? SuppressUndo { get; set; }
}