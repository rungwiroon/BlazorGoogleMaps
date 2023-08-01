using System.Text.Json.Serialization;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Maps.KmlLayer;

public class KmlLayerOptions
{
    /// <summary>
    /// The URL of the KML document to display.
    /// </summary>
    public string? Url { get; set; }
    /// <summary>
    /// Suppress the rendering of info windows when layer features are clicked.
    /// </summary>
    public bool? SuppressInfoWindows { get; set; }
    /// <summary>
    /// Whether to render the screen overlays.
    /// </summary>
    public bool? ScreenOverlays { get; set; }
    /// <summary>
    /// If this option is set to true or if the map's center and zoom were never set, the input map is centered and zoomed to the bounding box of the contents of the layer.
    /// </summary>
    public bool? PreserveViewport { get; set; }
    /// <summary>
    /// If true, the layer receives mouse events.
    /// </summary>
    public bool? Clickable { get; set; }

    /// <summary>
    /// Map on which to display the Entity.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map Map { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    public int? ZIndex { get; set; }
}