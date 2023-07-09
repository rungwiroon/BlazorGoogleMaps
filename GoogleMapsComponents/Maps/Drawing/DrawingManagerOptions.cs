using System.Text.Json.Serialization;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Maps.Drawing;

/// <summary>
/// Options for the drawing manager.
/// </summary>
public class DrawingManagerOptions
{
    /// <summary>
    /// Options to apply to any new circles created with this DrawingManager. 
    /// The center and radius properties are ignored, and the map property of a new circle is always set to the DrawingManager's map.
    /// </summary>
    public CircleOptions CircleOptions { get; set; }

    /// <summary>
    /// The enabled/disabled state of the drawing control. Defaults to true.
    /// </summary>
    public bool? DrawingControl { get; set; }

    /// <summary>
    /// The display options for the drawing control
    /// </summary>
    public DrawingControlOptions DrawingControlOptions { get; set; }

    /// <summary>
    /// The DrawingManager's drawing mode, which defines the type of overlay to be added on the map. 
    /// Accepted values are 'marker', 'polygon', 'polyline', 'rectangle', 'circle', or null. 
    /// A drawing mode of null means that the user can interact with the map as normal, and clicks do not draw anything.
    /// </summary>
    public OverlayType? DrawingMode { get; set; }

    /// <summary>
    /// The Map to which the DrawingManager is attached, which is the Map on which the overlays created will be placed.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map Map { get; set; }

    /// <summary>
    /// Options to apply to any new markers created with this DrawingManager. 
    /// The position property is ignored, and the map property of a new marker is always set to the DrawingManager's map.
    /// </summary>
    public MarkerOptions MarkerOptions { get; set; }

    /// <summary>
    /// Options to apply to any new polygons created with this DrawingManager. 
    /// The paths property is ignored, and the map property of a new polygon is always set to the DrawingManager's map.
    /// </summary>
    public PolygonOptions PolygonOptions { get; set; }

    /// <summary>
    /// Options to apply to any new polylines created with this DrawingManager. 
    /// The path property is ignored, and the map property of a new polyline is always set to the DrawingManager's map.
    /// </summary>
    public PolylineOptions PolylineOptions { get; set; }

    /// <summary>
    /// Options to apply to any new rectangles created with this DrawingManager. 
    /// The bounds property is ignored, and the map property of a new rectangle is always set to the DrawingManager's map.
    /// </summary>
    public RectangleOptions RectangleOptions { get; set; }
}