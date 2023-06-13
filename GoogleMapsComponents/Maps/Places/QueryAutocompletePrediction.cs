using System.Diagnostics;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Represents a single query autocomplete prediction.
/// Requires the &libraries=places URL parameter
/// </summary>
[DebuggerDisplay("{PlaceId, nq}: {Description, nq}")]
public class QueryAutocompletePrediction
{
    /// <summary>
    /// This is the unformatted version of the query suggested by the Places service.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// A set of substrings in the place's <see cref="Description"></see> that match elements in the user's input,
    /// suitable for use in highlighting those substrings.
    /// Each substring is identified by an offset and a length, expressed in unicode characters.
    /// </summary>
    [JsonPropertyName("matched_substrings")]
    public PredictionSubstring[] MatchedSubStrings { get; set; } = default!;

    /// <summary>
    /// Information about individual terms in the <see cref="Description"></see>, from most to least specific.
    /// For example, "Taco Bell", "Willitis", and "CA".
    /// </summary>
    public PredictionTerm[] Terms { get; set; } = default!;

    /// <summary>
    /// (optional) Only available if prediction is a place. A place ID that can be used to retrieve details about this place using the place details service or <see cref="Geocoder"></see>.
    /// </summary>
    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; } = default!;
}