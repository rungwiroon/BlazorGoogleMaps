using System.Text.Json.Serialization;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// A Place details query to be sent to the <see cref="PlacesService"></see>.
/// </summary>
public class PlaceDetailsRequest
{
    /// <summary>
    /// The Place ID of the Place for which details are being requested.
    /// </summary>
    public string PlaceId { get; set; } = default!;

    /// <summary>
    /// Fields to be included in the response, which will be billed for.
    /// If ['ALL'] is passed in, all available fields will be returned and billed for (this is not recommended for production deployments).
    /// For a list of fields see <see cref="PlaceResult"></see>.
    /// Nested fields can be specified with dot-paths (for example, "geometry.location").
    /// </summary>
    public string[] Fields { get; set; } = default!;

    /// <summary>
    /// (optional) A language identifier for the language in which the results should be returned, if possible.
    /// Results in the selected language may be given a higher ranking, but suggestions are not restricted to this language. 
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// (optional) A region code of the user's region.
    /// This can affect which photos may be returned, and possibly other things.
    /// The region code accepts a ccTLD ("top-level domain") two-character value.
    /// Most ccTLD codes are identical to ISO 3166-1 codes, with some notable exceptions.
    /// For example, the United Kingdom's ccTLD is "uk" (.co.uk) while its ISO 3166-1 code is "gb"
    /// (technically for the entity of "The United Kingdom of Great Britain and Northern Ireland").
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// (optional) Unique reference used to bundle the details request with an autocomplete session.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<AutocompleteSessionToken>))]
    public AutocompleteSessionToken? SessionToken { get; set; }
}