using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

public class FullscreenControlOptions
{
    /// <summary>
    /// Position id. Used to specify the position of the control on the map.<br/>
    /// The default position is <c>RightTop</c>.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ControlPosition>))]
    public ControlPosition Position { get; set; }
}