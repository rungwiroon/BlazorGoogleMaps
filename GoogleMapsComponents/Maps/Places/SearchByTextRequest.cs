namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Request for searchByText method
/// https://developers.google.com/maps/documentation/javascript/reference/place#SearchByTextRequest
/// </summary>
public class SearchByTextRequest
{
    /// <summary>
    /// The text query for textual search.
    /// </summary>
    public string? TextQuery { get; set; }

    /// <summary>
    /// Fields to be included for fetched places in the response.
    /// For a list of fields see Place class.
    /// Nested fields can be specified with dot-paths (for example, "displayName", "formattedAddress").
    /// </summary>
    public required string[]? Fields { get; set; }

    /// <summary>
    /// (Optional) A language identifier for the language in which names and addresses should be returned, when possible.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// (Optional) The region code, specified as a ccTLD ("top-level domain") two-character value. 
    /// Most ccTLD codes are identical to ISO 3166-1 codes.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// (Optional) The Unicode country/region code (CLDR) of the location where the request is coming from.
    /// </summary>
    public string? IncludedType { get; set; }

    /// <summary>
    /// (Optional) Place details will be displayed with the preferred language if available.
    /// </summary>
    public LatLngLiteral? LocationBias { get; set; }

    /// <summary>
    /// (Optional) The maximum number of results to return. Must be between 1 and 20, default to 20.
    /// </summary>
    public int? MaxResultCount { get; set; }

    /// <summary>
    /// (Optional) Used to restrict the search to places that are currently open.
    /// </summary>
    public bool? IsOpenNow { get; set; }

    /// <summary>
    /// (Optional) Used to restrict the search to places that are marked as certain price levels.
    /// </summary>
    public int[]? PriceLevels { get; set; }

    /// <summary>
    /// (Optional) How results will be ranked in the response.
    /// </summary>
    public string? RankPreference { get; set; }
}