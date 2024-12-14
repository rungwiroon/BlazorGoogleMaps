using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Built-in symbol paths.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SymbolPath
{
    /// <summary>
    /// A backward-pointing closed arrow.
    /// </summary>
    [EnumMember(Value = "BACKWARD_CLOSED_ARROW")]
    BACKWARD_CLOSED_ARROW = 3,

    /// <summary>
    /// A backward-pointing open arrow.
    /// </summary>
    [EnumMember(Value = "BACKWARD_OPEN_ARROW")]
    BACKWARD_OPEN_ARROW = 4,

    /// <summary>
    /// A circle.
    /// </summary>
    [EnumMember(Value = "CIRCLE")]
    CIRCLE = 0,

    /// <summary>
    /// A forward-pointing closed arrow.
    /// </summary>
    [EnumMember(Value = "FORWARD_CLOSED_ARROW")]
    FORWARD_CLOSED_ARROW = 1,

    /// <summary>
    /// A forward-pointing open arrow.
    /// </summary>
    [EnumMember(Value = "FORWARD_OPEN_ARROW")]
    FORWARD_OPEN_ARROW = 2
}