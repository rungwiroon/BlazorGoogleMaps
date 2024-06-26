using GoogleMapsComponents.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

public class TrafficLayerOptions
{
    public bool AutoRefresh { get; set; }
    /// <summary>
    /// Map on which to display the Entity.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map? Map { get; set; }
}