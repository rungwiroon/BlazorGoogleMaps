using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// An Autocomplete response returned by the call to <see cref="AutocompleteService.GetPlacePredictions"></see>
/// containing a list of <see cref="AutocompletePrediction"></see>s.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class AutocompleteResponse
{
    /// <summary>
    /// The list of <see cref="AutocompletePrediction"></see>s.
    /// </summary>
    public AutocompletePrediction[] Predictions { get; set; } = new AutocompletePrediction[] { };

    [JsonConverter(typeof(EnumMemberConverter<PlaceServiceStatus>))]
    public PlaceServiceStatus Status { get; set; }
}