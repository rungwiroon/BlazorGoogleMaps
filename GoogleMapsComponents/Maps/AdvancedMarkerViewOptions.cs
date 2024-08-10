using GoogleMapsComponents.Serialization;
using OneOf;
using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GoogleMapsComponents.Maps;

public class AdvancedMarkerElementOptions : IListableEntityOptionsBase
{
    /// <summary>
    /// Sets the AdvancedMarkerElement's position. An AdvancedMarkerElement may be constructed without a position, but will not be displayed until its position is provided - for example, by a user's actions or choices. An AdvancedMarkerElement's position can be provided by setting AdvancedMarkerElement.position if not provided at the construction.
    /// Note: AdvancedMarkerElement with altitude is only supported on vector maps.
    /// </summary>
    public LatLngLiteral? Position { get; set; }

    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map? Map { get; set; }

    /// <summary>
    /// An enumeration specifying how an AdvancedMarkerElement should behave when it collides with another AdvancedMarkerElement or with the basemap labels on a vector map.
    /// Note: AdvancedMarkerElement to AdvancedMarkerElement collision works on both raster and vector maps, however, AdvancedMarkerElement to base map's label collision only works on vector maps.
    /// </summary>

    [JsonConverter(typeof(JsonStringEnumConverterEx<CollisionBehavior>))]
    public CollisionBehavior? CollisionBehavior { get; set; }

    /// <summary>
    /// string to set up HTMLELement like svg, img, div, etc.
    /// PinElement to set up a pin element with url or text.
    /// </summary>
    public OneOf<string, PinElement> Content { get; set; }

    /// <summary>
    /// If true, the AdvancedMarkerElement can be dragged.
    /// Note: AdvancedMarkerElement with altitude is not draggable.
    /// </summary>
    public bool GmpDraggable { get; set; }

    /// <summary>
    /// If true, the AdvancedMarkerElement will be clickable and trigger the gmp-click event, and will be interactive for accessibility purposes (e.g. allowing keyboard navigation via arrow keys).
    /// </summary>
    public bool GmpClickable { get; set; }

    /// <summary>
    /// Rollover text. If provided, an accessibility text (e.g. for use with screen readers) will be added to the
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    public int? ZIndex { get; set; }
}
