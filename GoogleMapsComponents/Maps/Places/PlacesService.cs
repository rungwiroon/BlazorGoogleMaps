using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Contains methods related to searching for places and retrieving details about a place.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class PlacesService
{
    private readonly JsObjectRef _jsObjectRef;

    /// <summary>
    /// Creates a new instance of the PlacesService that renders attributions in the specified container.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="attrContainer"></param>
    /// <returns></returns>
    public static async Task<PlacesService> CreateAsync(IJSRuntime jsRuntime, OneOf<ElementReference, Map> attrContainer)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.PlacesService", attrContainer);
        var obj = new PlacesService(jsObjectRef);

        return obj;
    }

    private PlacesService(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    /// <summary>
    /// Retrieves a list of places based on a phone number. In most cases there should be just one item in the result list,
    /// however if the request is ambiguous more than one result may be returned.
    /// The PlaceResults passed to the callback are subsets of a full PlaceResult.
    /// Your app can get a more detailed PlaceResult for each place by calling PlacesService.getDetails and passing the PlaceResult.place_id for the desired place.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaceResponse> FindPlaceFromPhoneNumber(FindPlaceFromPhoneNumberRequest request)
    {
        return await _jsObjectRef.InvokeAsync<PlaceResponse>("findPlaceFromPhoneNumber", request);
    }

    /// <summary>
    /// Retrieves a list of places based on a query string. In most cases there should be just one item in the result list,
    /// however if the request is ambiguous more than one result may be returned.
    /// The PlaceResults passed to the callback are subsets of a full PlaceResult.
    /// Your app can get a more detailed PlaceResult for each place by calling PlacesService.getDetails and passing the PlaceResult.place_id for the desired place.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaceResponse> FindPlaceFromQuery(FindPlaceFromQueryRequest request)
    {
        return await _jsObjectRef.InvokeAsync<PlaceResponse>("findPlaceFromQuery", request);
    }

    /// <summary>
    /// Retrieves details about the place identified by the given placeId.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaceResponse> GetDetails(PlaceDetailsRequest request)
    {
        return await _jsObjectRef.InvokeAsync<PlaceResponse>("getDetails", request);
    }

    public void Dispose()
    {
        _jsObjectRef.Dispose();
    }
}