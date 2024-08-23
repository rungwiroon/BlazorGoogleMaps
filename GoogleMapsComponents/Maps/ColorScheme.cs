using System.Runtime.Serialization;

namespace GoogleMapsComponents.Maps;

public enum ColorScheme
{
    /// <summary>
    /// The dark color scheme for a map.
    /// </summary>
    [EnumMember(Value = "DARK")]
    Dark,
    /// <summary>
    /// The color scheme is selected based on system preferences.
    /// </summary>
    [EnumMember(Value = "FOLLOW_SYSTEM")]
    FollowSystem,
    /// <summary>
    /// The light color scheme for a map. Default value for legacy Maps JS.
    /// </summary>
    [EnumMember(Value = "LIGHT")]
    Light
}