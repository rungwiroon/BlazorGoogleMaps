namespace GoogleMapsComponents.Maps;

/// <summary>
/// A representation of duration as a numeric value and a display string.
/// </summary>
public class Duration
{
    /// <summary>
    /// A string representation of the duration value.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// The duration in seconds.
    /// </summary>
    public float Value { get; set; }
}