using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Controls;

public class MotionTrackingControlOptions
{
    /// <summary>
    /// Position id. Used to specify the position of the control on the map.<br/>
    /// The default position is <c>RightBottom</c>.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ControlPosition>))]
    public ControlPosition Position { get; set; }
}