using GoogleMapsComponents.Serialization;
using OneOf;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// An Autocompletion request to be sent to <see cref="AutocompleteService.GetPlacePredictions"></see>.
/// </summary>
public class AutocompletionRequest
{
    /// <summary>
    /// The user entered input string.
    /// </summary>
    public string Input { get; set; } = default!;

    /// <summary>
    /// (optional) Bounds for prediction biasing. Predictions will be biased towards, but not restricted to, the given bounds.
    /// Both location and radius will be ignored if bounds is set.
    /// </summary>
    public OneOf<LatLngBounds, LatLngBoundsLiteral>? Bounds { get; set; }

    /// <summary>
    /// (optional) The component restrictions. Component restrictions are used to restrict predictions to only those within the parent component.
    /// For example, the country.
    /// See list of supported languages at:
    /// https://developers.google.com/maps/faq#languagesupport
    /// </summary>
    public ComponentRestrictions? ComponentRestrictions { get; set; }

    /// <summary>
    /// (optional) A language identifier for the language in which the results should be returned, if possible.
    /// Results in the selected language may be given a higher ranking, but suggestions are not restricted to this language. 
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// (optional) Location for prediction biasing. Predictions will be biased towards the given location and radius.
    /// Alternatively, bounds can be used.
    /// </summary>
    public LatLngBoundsLiteral? Location { get; set; }

    /// <summary>
    /// (optional) The character position in the input term at which the service uses text for predictions (the position of the cursor in the input field).
    /// </summary>
    public int? Offset { get; set; }

    /// <summary>
    /// (optional) The location where <see cref="AutocompletePrediction.DistanceInMeters"></see> is calculated from.
    /// </summary>
    public OneOf<LatLngBounds, LatLngBoundsLiteral>? Origin { get; set; }

    /// <summary>
    /// (optional) The radius of the area used for prediction biasing. The radius is specified in meters, and must always be accompanied by a location property.
    /// Alternatively, bounds can be used.
    /// </summary>
    public decimal? Radius { get; set; }

    /// <summary>
    /// (optional) A region code which is used for result formatting and for result filtering.
    /// It does not restrict the suggestions to this country.
    /// The region code accepts a ccTLD ("top-level domain") two-character value.
    /// Most ccTLD codes are identical to ISO 3166-1 codes, with some notable exceptions.
    /// For example, the United Kingdom's ccTLD is "uk" (.co.uk) while its ISO 3166-1 code is "gb"
    /// (technically for the entity of "The United Kingdom of Great Britain and Northern Ireland").
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// (optional) Unique reference used to bundle individual requests into sessions.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<AutocompleteSessionToken>))]
    public AutocompleteSessionToken? SessionToken { get; set; }

    /// <summary>
    /// (optional) The types of predictions to be returned.
    /// If no types are specified, all types will be returned.
    /// For supported types, see the developer's guide at:
    /// https://developers.google.com/maps/documentation/javascript/place-autocomplete#constrain-place-types
    /// </summary>
    public string[]? Types { get; set; }
}