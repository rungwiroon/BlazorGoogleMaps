namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Represents a prediction term.
/// </summary>
public class PredictionTerm
{
    /// <summary>
    /// The offset, in unicode characters, of the start of this term in the description of the place.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// The value of this term, for example, "Taco Bell".
    /// </summary>
    public string Value { get; set; } = default!;
}