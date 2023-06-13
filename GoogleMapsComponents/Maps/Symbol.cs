using OneOf;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Describes a symbol, which consists of a vector path with styling. 
/// A symbol can be used as the icon of a marker, or placed on a polyline.
/// </summary>
public class Symbol
{
    public OneOf<SymbolPath, string> Path { get; set; }
    /// <summary>
    /// The position of the symbol relative to the marker or polyline. 
    /// The coordinates of the symbol's path are translated left and up by the anchor's x and y coordinates respectively. 
    /// By default, a symbol is anchored at (0, 0). 
    /// The position is expressed in the same coordinate system as the symbol's path.
    /// </summary>
    public Point Anchor { get; set; }

    /// <summary>
    /// The symbol's fill color. All CSS3 colors are supported except for extended named colors. 
    /// For symbol markers, this defaults to 'black'. 
    /// For symbols on polylines, this defaults to the stroke color of the corresponding polyline.
    /// </summary>
    public string FillColor { get; set; }

    /// <summary>
    /// The symbol's fill opacity. Defaults to 0.
    /// </summary>
    public float? FillOpacity { get; set; }

    /// <summary>
    /// The origin of the label relative to the origin of the path, if label is supplied by the marker. 
    /// By default, the origin is located at (0, 0). 
    /// The origin is expressed in the same coordinate system as the symbol's path. 
    /// This property is unused for symbols on polylines.
    /// </summary>
    public Point LabelOrigin { get; set; }

    /// <summary>
    /// The angle by which to rotate the symbol, expressed clockwise in degrees. Defaults to 0. 
    /// A symbol in an IconSequence where fixedRotation is false is rotated relative to the angle of the edge on which it lies.
    /// </summary>
    public float? Rotation { get; set; }

    /// <summary>
    /// The angle by which to rotate the symbol, expressed clockwise in degrees. Defaults to 0. 
    /// A symbol in an IconSequence where fixedRotation is false is rotated relative to the angle of the edge on which it lies.
    /// </summary>
    public float? Scale { get; set; }

    /// <summary>
    /// The symbol's stroke color. All CSS3 colors are supported except for extended named colors. 
    /// For symbol markers, this defaults to 'black'. 
    /// For symbols on a polyline, this defaults to the stroke color of the polyline.
    /// </summary>
    public string StrokeColor { get; set; }

    /// <summary>
    /// The symbol's stroke opacity. For symbol markers, this defaults to 1. 
    /// For symbols on a polyline, this defaults to the stroke opacity of the polyline.
    /// </summary>
    public float? StrokeOpacity { get; set; }

    /// <summary>
    /// The symbol's stroke weight. Defaults to the scale of the symbol.
    /// </summary>
    public float? StrokeWeight { get; set; }
}