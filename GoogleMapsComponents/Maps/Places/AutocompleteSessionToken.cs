using Microsoft.JSInterop;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

public class AutocompleteSessionToken : IDisposable, IJsObjectRef
{
    private readonly JsObjectRef _jsObjectRef;

    [JsonPropertyName("GuidString")]
    public Guid Guid => _jsObjectRef.Guid;

    public static async Task<AutocompleteSessionToken> CreateAsync(IJSRuntime jsRuntime)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.AutocompleteSessionToken");
        var obj = new AutocompleteSessionToken(jsObjectRef);

        return obj;
    }

    private AutocompleteSessionToken(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    public void Dispose()
    {
        _jsObjectRef.Dispose();
    }
}