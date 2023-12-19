using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Options for the rendering of the Street View pegman control on the map.
/// </summary>
public class StreetViewControlOptions
{
    /// <summary>
    /// Position id. Used to specify the position of the control on the map.<br/>
    /// The default position is embedded within the navigation (zoom and pan) controls.<br/>
    /// If this position is empty or the same as that specified in the <c>ZoomControlOptions</c> or <c>PanControlOptions</c>, the Street View control will be displayed as part of the navigation controls.
    /// Otherwise, it will be displayed separately.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<ControlPosition>))]
    public ControlPosition Position { get; set; }
}