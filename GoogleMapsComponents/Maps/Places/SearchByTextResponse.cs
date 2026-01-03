namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Response from searchByText method
/// https://developers.google.com/maps/documentation/javascript/reference/place#SearchByTextResponse
/// </summary>
public class SearchByTextResponse
{
    /// <summary>
    /// A list of places that match the search request.
    /// </summary>
    public Place[]? Places { get; set; }
}
