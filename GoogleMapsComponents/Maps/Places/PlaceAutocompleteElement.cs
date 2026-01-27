using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

public class PlaceAutocompleteElement : EventEntityBase
{
    public static async Task<PlaceAutocompleteElement> CreateAsync(IJSRuntime jsRuntime, object? options = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.PlaceAutocompleteElement", options);
        return new PlaceAutocompleteElement(jsObjectRef);
    }

    public static async Task<PlaceAutocompleteElement> FromElementAsync(IJSRuntime jsRuntime, ElementReference element)
    {
        var guid = await jsRuntime.InvokeAsync<string>("blazorGoogleMaps.objectManager.addObject", element);
        var jsObjectRef = new JsObjectRef(jsRuntime, new Guid(guid));
        return new PlaceAutocompleteElement(jsObjectRef);
    }

    private PlaceAutocompleteElement(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    public Task SetPropertyAsync(string propertyName, object? value)
    {
        return _jsObjectRef.InvokePropertyAsync(propertyName, value);
    }

    public Task<T?> GetPropertyAsync<T>(string propertyName)
    {
        return _jsObjectRef.InvokePropertyAsync<T>(propertyName);
    }

    public async Task<string?> GetInputValueAsync()
    {
        return await _jsObjectRef.JSRuntime.InvokeAsync<string?>(
            "blazorGoogleMaps.objectManager.readPlaceAutocompleteInputValue",
            _jsObjectRef.Guid.ToString());
    }
}
