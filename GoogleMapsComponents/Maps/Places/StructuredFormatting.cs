using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Contains structured information about the place's description,
/// divided into a main text and a secondary text, including an array of matched substrings from the autocomplete input,
/// identified by an offset and a length, expressed in unicode characters.
/// </summary>
public class StructuredFormatting
{
    /// <summary>
    /// This is the main text part of the unformatted description of the place suggested by the Places service.
    /// Usually the name of the place.
    /// </summary>
    [JsonPropertyName("main_text")]
    public string MainText { get; set; } = default!;

    /// <summary>
    /// A set of substrings in the main text that match elements in the user's input, suitable for use in highlighting those substrings.
    /// Each substring is identified by an offset and a length, expressed in unicode characters.
    /// </summary>
    [JsonPropertyName("main_text_matched_substrings")]
    public PredictionSubstring[]
        MainTextMatchedSubstrings
    { get; set; } = default!;

    /// <summary>
    /// This is the secondary text part of the unformatted description of the place suggested by the Places service.
    /// Usually the location of the place.
    /// </summary>
    [JsonPropertyName("secondary_text")]
    public string SecondaryText { get; set; } = default!;
}