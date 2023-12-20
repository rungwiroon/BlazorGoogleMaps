using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Options for the rendering of the scale control.
/// </summary>
public class ScaleControlOptions
{
    /// <summary>
    /// Style id. Used to select what style of scale control to display.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ScaleControlStyle>))]
    public ScaleControlStyle Style { get; set; }
}