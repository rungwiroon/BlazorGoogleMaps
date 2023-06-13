namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Represents a prediction substring.
/// </summary>
public class PredictionSubstring
{
    /// <summary>
    /// The length of the substring.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// The offset to the substring's start within the description string.
    /// </summary>
    public int Length { get; set; }
}