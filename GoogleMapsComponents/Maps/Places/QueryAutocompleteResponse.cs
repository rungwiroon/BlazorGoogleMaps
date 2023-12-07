using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// An Autocomplete response returned by the call to <see cref="AutocompleteService.GetQueryPredictions"></see>
/// containing a list of <see cref="AutocompletePrediction"></see>s.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class QueryAutocompleteResponse
{
    /// <summary>
    /// The list of <see cref="AutocompletePrediction"></see>s.
    /// </summary>
    public QueryAutocompletePrediction[] Predictions { get; set; } = new QueryAutocompletePrediction[] { };

    [JsonConverter(typeof(EnumMemberConverter<PlaceServiceStatus>))]
    public PlaceServiceStatus Status { get; set; }
}