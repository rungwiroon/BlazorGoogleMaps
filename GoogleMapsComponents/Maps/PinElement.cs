namespace GoogleMapsComponents.Maps;

/// <summary>
/// A PinElement represents a DOM element that consists of a shape and a glyph. The shape has the same balloon style as seen in the default AdvancedMarkerElement. The glyph is an optional DOM element displayed in the balloon shape. A PinElement may have a different aspect ratio depending on its PinElement.scale.
/// https://developers.google.com/maps/documentation/javascript/reference/advanced-markers#PinElement.element
/// </summary>
public class PinElement
{
    public string? Background { get; set; }
    public string? BorderColor { get; set; }
    public string? Glyph { get; set; }
    public string? GlyphColor { get; set; }
    public double? Scale { get; set; }

    public PinElement()
    {

    }
}