namespace GoogleMapsComponents.Maps;

public class GroundOverlayOptions
{
    /// <summary>
    /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
    /// </summary>
    public double Opacity { get; set; } = 1;

    /// <summary>
    /// If true, the ground overlay can receive mouse events.
    /// </summary>
    public bool Clickable { get; set; }
}