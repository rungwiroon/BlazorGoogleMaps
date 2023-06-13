using GoogleMapsComponents.Maps.Drawing;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// The properties of an overlaycomplete event on a DrawingManager.
/// https://developers.google.com/maps/documentation/javascript/reference/drawing#OverlayCompleteEvent
/// </summary>
public class OverlayCompleteEvent
{
    public Polygon? Polygon { get; set; }
    public Marker? Marker { get; set; }
    public Polyline? Polyline { get; set; }
    public Rectangle? Rectangle { get; set; }
    public Circle? Circle { get; set; }

    /// <summary>
    /// The completed overlay's type.
    /// </summary>
    public OverlayType Type { get; set; }
}