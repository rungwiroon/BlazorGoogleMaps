using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

public class AutocompleteService : IDisposable
{
    private readonly JsObjectRef _jsObjectRef;

    public static async Task<AutocompleteService> CreateAsync(IJSRuntime jsRuntime)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.AutocompleteService");
        var obj = new AutocompleteService(jsObjectRef);

        return obj;
    }

    private AutocompleteService(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    /// <summary>
    /// Retrieves place <see cref="AutocompletePrediction"></see>s based on the supplied <see cref="AutocompletionRequest"></see>.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<AutocompleteResponse> GetPlacePredictions(AutocompletionRequest request)
    {
        return await _jsObjectRef.InvokeAsync<AutocompleteResponse>("getPlacePredictions", request);
    }

    /// <summary>
    /// Retrieves <see cref="QueryAutocompletePrediction"></see>s based on the supplied <see cref="QueryAutocompletionRequest"></see>.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QueryAutocompleteResponse> GetQueryPredictions(QueryAutocompletionRequest request)
    {
        return await _jsObjectRef.InvokeAsync<QueryAutocompleteResponse>("getQueryPredictions", request);
    }

    public void Dispose()
    {
        _jsObjectRef.Dispose();
    }
}