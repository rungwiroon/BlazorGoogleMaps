using System.Runtime.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Built-in symbol paths.
/// </summary>
public enum SymbolPath
{
    /// <summary>
    /// A backward-pointing closed arrow.
    /// </summary>
    [EnumMember(Value = "3")]
    BACKWARD_CLOSED_ARROW = 3,

    /// <summary>
    /// A backward-pointing open arrow.
    /// </summary>
    [EnumMember(Value = "4")]
    BACKWARD_OPEN_ARROW = 4,

    /// <summary>
    /// A circle.
    /// </summary>
    [EnumMember(Value = "0")]
    CIRCLE = 0,

    /// <summary>
    /// A forward-pointing closed arrow.
    /// </summary>
    [EnumMember(Value = "1")]
    FORWARD_CLOSED_ARROW = 1,

    /// <summary>
    /// A forward-pointing open arrow.
    /// </summary>
    [EnumMember(Value = "2")]
    FORWARD_OPEN_ARROW = 2
}