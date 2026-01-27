using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// https://developers.google.com/maps/documentation/javascript/place
/// https://developers.google.com/maps/documentation/javascript/place-search
/// </summary>
public class PlaceService
{
    private readonly JsObjectRef _jsObjectRef;

    /// <summary>
    /// Creates a new instance of the PlacesService that renders attributions in the specified container.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="placeOptions"></param>
    /// <returns></returns>
    public static async Task<PlaceService> CreateAsync(IJSRuntime jsRuntime, PlaceOptions? placeOptions = null)
    {
        const string jsObjectName = "google.maps.places.Place";
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, jsObjectName, placeOptions);
        var obj = new PlaceService(jsObjectRef);

        return obj;
    }

    private PlaceService(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    /// <summary>
    /// https://developers.google.com/maps/documentation/javascript/reference/place#Place.fetchFields
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Place> FetchFields(FetchFieldsRequest request)
    {
        return await _jsObjectRef.InvokeAsync<Place>("fetchFields", request);
    }

    /// <summary>
    /// Fetches a list of places based on text search.
    /// https://developers.google.com/maps/documentation/javascript/reference/place#Place.searchByText
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<SearchByTextResponse> SearchByText(SearchByTextRequest request)
    {
        return await _jsObjectRef.InvokeAsync<SearchByTextResponse>("searchByText", request);
    }
}
