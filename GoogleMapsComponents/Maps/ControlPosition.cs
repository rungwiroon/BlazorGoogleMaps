using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Identifiers used to specify the placement of controls on the map.<br/>
/// Controls are positioned relative to other controls in the same layout position.<br/>
/// Controls that are added first are positioned closer to the edge of the map.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ControlPosition
{
    /// <summary>
    /// Elements are positioned in the center of the bottom row.
    /// </summary>
    [EnumMember(Value = "BOTTOM_CENTER")]
    BottomCenter,

    /// <summary>
    /// Elements are positioned in the bottom left and flow towards the middle.<br/>
    /// Elements are positioned to the right of the Google logo.
    /// </summary>
    [EnumMember(Value = "BOTTOM_LEFT")]
    BottomLeft,

    /// <summary>
    /// Elements are positioned in the bottom right and flow towards the middle.<br/>
    /// Elements are positioned to the left of the copyrights.
    /// </summary>
    [EnumMember(Value = "BOTTOM_RIGHT")]
    BottomRight,

    /// <summary>
    /// Elements are positioned on the left, above bottom-left elements, and flow upwards.
    /// </summary>
    [EnumMember(Value = "LEFT_BOTTOM")]
    LeftBottom,

    /// <summary>
    /// Elements are positioned in the center of the left side.
    /// </summary>
    [EnumMember(Value = "LEFT_CENTER")]
    LeftCenter,

    /// <summary>
    /// Elements are positioned on the left, below top-left elements, and flow downwards.
    /// </summary>
    [EnumMember(Value = "LEFT_TOP")]
    LeftTop,

    /// <summary>
    /// Elements are positioned on the right, above bottom-right elements, and flow upwards.
    /// </summary>
    [EnumMember(Value = "RIGHT_BOTTOM")]
    RightBottom,

    /// <summary>
    /// Elements are positioned in the center of the right side.
    /// </summary>
    [EnumMember(Value = "RIGHT_CENTER")]
    RightCenter,

    /// <summary>
    /// Elements are positioned on the right, below top-right elements, and flow downwards.
    /// </summary>
    [EnumMember(Value = "RIGHT_TOP")]
    RightTop,

    /// <summary>
    /// Elements are positioned in the center of the top row.
    /// </summary>
    [EnumMember(Value = "TOP_CENTER")]
    TopCenter,

    /// <summary>
    /// Elements are positioned in the top left and flow towards the middle.
    /// </summary>
    [EnumMember(Value = "TOP_LEFT")]
    TopLeft,

    /// <summary>
    /// Elements are positioned in the top right and flow towards the middle.
    /// </summary>
    [EnumMember(Value = "TOP_RIGHT")]
    TopRight
}