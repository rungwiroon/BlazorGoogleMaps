using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.Drawing;

/// <summary>
/// Options for the rendering of the drawing control.
/// </summary>
public class DrawingControlOptions
{
    /// <summary>
    /// The drawing modes to display in the drawing control, in the order in which they are to be displayed. 
    /// The hand icon (which corresponds to the null drawing mode) is always available and is not to be specified in this array. 
    /// Defaults to ['marker', 'polyline', 'rectangle', 'circle', 'polygon'].
    /// </summary>
    public IEnumerable<OverlayType> DrawingModes { get; set; }

    /// <summary>
    /// Position id. Used to specify the position of the control on the map. 
    /// The default position is TOP_LEFT.
    /// </summary>
    public ControlPosition Position { get; set; }
}