using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Options for the rendering of the rotate control.
/// </summary>
public class RotateControlOptions
{
    /// <summary>
    /// Position id. Used to specify the position of the control on the map.
    /// The default position is <c>LeftBottom</c>.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ControlPosition>))]
    public ControlPosition Position { get; set; }
}