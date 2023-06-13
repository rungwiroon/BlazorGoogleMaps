using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using GoogleMapsComponents.Maps.Places;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A service for converting between an address and a LatLng.
/// </summary>
public class Geocoder : IDisposable
{
    private readonly JsObjectRef _jsObjectRef;

    /// <summary>
    /// Creates a new instance of a Geocoder that sends geocode requests to Google servers.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <returns></returns>
    public async static Task<Geocoder> CreateAsync(IJSRuntime jsRuntime)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Geocoder");
        var obj = new Geocoder(jsObjectRef);

        return obj;
    }

    /// <summary>
    /// Creates a new instance of a Geocoder that sends geocode requests to Google servers.
    /// </summary>
    /// <param name="jsObjectRef"></param>
    private Geocoder(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    /// <summary>
    /// Geocode a request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<GeocoderResponse> Geocode(GeocoderRequest request)
    {
        return await _jsObjectRef.InvokeAsync<GeocoderResponse>("geocode", request);
    }

    public void Dispose()
    {
        _jsObjectRef.Dispose();
    }
}