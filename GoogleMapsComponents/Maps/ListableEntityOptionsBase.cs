using GoogleMapsComponents.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

public interface IListableEntityOptionsBase
{
    Map? Map { get; set; }
}

public abstract class ListableEntityOptionsBase : IListableEntityOptionsBase
{
    /// <summary>
    /// Indicates whether Entity handles mouse events.
    /// Defaults to true.
    /// </summary>
    public bool? Clickable { get; set; }

    /// <summary>
    /// If set to true, the user can drag Entity over the map.
    /// Defaults to false.
    /// </summary>
    public bool? Draggable { get; set; }

    /// <summary>
    /// Map on which to display the Entity.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map? Map { get; set; }

    /// <summary>
    /// If true, the Entity is visible
    /// </summary>
    public bool? Visible { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    public int? ZIndex { get; set; }
}