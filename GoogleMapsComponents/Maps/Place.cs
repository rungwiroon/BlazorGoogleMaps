using OneOf;


namespace GoogleMapsComponents.Maps;

/// <summary>
/// Contains information needed to locate, identify, or describe a place for a DirectionsRequest or DistanceMatrixRequest. In this context, "place" means a business, point of interest, or geographic location. For fetching information about a place, see PlacesService.
/// </summary>
public class Place
{
    /// <summary>
    /// Type:  LatLng|LatLngLiteral optional
    /// The LatLng of the entity described by this place.
    /// </summary>
    public OneOf<string, LatLngLiteral> Location { get; set; }

    /// <summary>
    /// The place ID of the place (such as a business or point of interest). The place ID is a unique identifier of a place in the Google Maps database. Note that the placeId is the most accurate way of identifying a place. If possible, you should specify the placeId rather than a query. A place ID can be retrieved from any request to the Places API, such as a TextSearch. Place IDs can also be retrieved from requests to the Geocoding API. For more information, see the overview of place IDs.
    /// </summary>
    public string PlaceId { get; set; }

    /// <summary>
    /// A search query describing the place (such as a business or point of interest). An example query is "Quay, Upper Level, Overseas Passenger Terminal 5 Hickson Road, The Rocks NSW". If possible, you should specify the placeId rather than a query. The API does not guarantee the accuracy of resolving the query string to a place. If both the placeId and query are provided, an error occurs.
    /// </summary>
    public string Query { get; set; }
}