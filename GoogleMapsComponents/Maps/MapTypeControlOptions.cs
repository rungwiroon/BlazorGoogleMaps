using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Options for the rendering of the map type control.
/// </summary>
public class MapTypeControlOptions
{
    /// <summary>
    /// IDs of map types to show in the control.
    /// </summary>
    public MapTypeId[] MapTypeIds { get; set; }

    /// <summary>
    /// Position id. Used to specify the position of the control on the map.<br/>
    /// The default position is <c>TopLeft</c>.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ControlPosition>))]
    public ControlPosition Position { get; set; }

    /// <summary>
    /// Style id. Used to select what style of map type control to display.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<MapTypeControlStyle>))]
    public MapTypeControlStyle Style { get; set; }
}