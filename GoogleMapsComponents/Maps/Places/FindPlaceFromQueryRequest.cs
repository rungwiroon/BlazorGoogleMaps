namespace GoogleMapsComponents.Maps.Places;

public class FindPlaceFromQueryRequest : FindPlaceFromBase
{
    /// <summary>
    /// The request's query. For example, the name or address of a place.
    /// </summary>
    public string Query { get; set; } = default!;
}