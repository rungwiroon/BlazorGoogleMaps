using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// 2023-09
/// Notice: Available only in the v=beta channel.
/// </summary>
public class AdvancedMarkerElement : ListableEntityBase<AdvancedMarkerElementOptions>
{
    // https://developers.google.com/maps/documentation/javascript/reference/3.55/advanced-markers
    public const string GoogleMapAdvancedMarkerName = "google.maps.marker.AdvancedMarkerElement";

    public static async Task<AdvancedMarkerElement> CreateAsync(IJSRuntime jsRuntime, AdvancedMarkerElementOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, GoogleMapAdvancedMarkerName, opts);
        var obj = new AdvancedMarkerElement(jsObjectRef);
        return obj;
    }

    internal AdvancedMarkerElement(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>("getZIndex");
    }

    public async Task SetPosition(LatLngLiteral newPosition)
    {
        await _jsObjectRef.InvokePropertyAsync("position", newPosition);
    }

    public async Task SetContent(string newContent)
    {
        await _jsObjectRef.InvokePropertyAsync("content", newContent);
    }

    public async Task<LatLngLiteral> GetPosition()
    {
        return await _jsObjectRef.InvokePropertyAsync<LatLngLiteral>("position");
    }
}
