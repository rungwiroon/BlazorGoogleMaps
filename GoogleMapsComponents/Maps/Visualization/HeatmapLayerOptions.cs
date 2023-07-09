using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Maps.Visualization;

/// <summary>
/// This object defines the properties that can be set on a HeatmapLayer object.
/// </summary>
public class HeatmapLayerOptions
{
    /// <summary>
    /// The data points to display. Required.
    /// </summary>
    public IEnumerable<LatLngLiteral> Data { get; set; }

    /// <summary>
    /// Specifies whether heatmaps dissipate on zoom. By default, the radius of influence of a data point is specified by the radius option only. 
    /// When dissipating is disabled, the radius option is interpreted as a radius at zoom level 0.
    /// </summary>
    public bool? Dissipating { get; set; }

    /// <summary>
    /// The color gradient of the heatmap, specified as an array of CSS color strings. 
    /// All CSS3 colors are supported except for extended named colors.
    /// </summary>
    public IEnumerable<string> Gradient { get; set; }

    /// <summary>
    /// The map on which to display the layer.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map? Map { get; set; }

    /// <summary>
    /// The maximum intensity of the heatmap. 
    /// By default, heatmap colors are dynamically scaled according to the greatest concentration of points at any particular pixel on the map. 
    /// This property allows you to specify a fixed maximum.
    /// </summary>
    public float? MaxIntensity { get; set; }

    /// <summary>
    /// The opacity of the heatmap, expressed as a number between 0 and 1. Defaults to 0.6.
    /// </summary>
    public float? Opacity { get; set; }

    /// <summary>
    /// The radius of influence for each data point, in pixels.
    /// </summary>
    public float? Radius { get; set; }
}