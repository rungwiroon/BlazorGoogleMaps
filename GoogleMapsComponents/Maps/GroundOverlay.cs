using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public class GroundOverlay : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;

    public static async Task<GroundOverlay> CreateAsync(IJSRuntime jsRuntime, string url, LatLngBoundsLiteral bounds, GroundOverlayOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.GroundOverlay", url, bounds, opts);
        var obj = new GroundOverlay(jsObjectRef);

        return obj;
    }

    internal GroundOverlay(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    /// <summary>
    /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
    /// </summary>
    /// <param name="opacity"></param>
    /// <returns></returns>
    public async Task SetOpacity(double opacity)
    {
        if (opacity > 1) return;
        if (opacity < 0) return;

        await _jsObjectRef.InvokeAsync("setOpacity", opacity);
    }

    /// <summary>
    /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
    /// </summary>
    /// <param name="opacity"></param>
    /// <returns></returns>
    public async Task SetOpacity(decimal opacity)
    {
        await SetOpacity(Convert.ToDouble(opacity));
    }

    public async Task SetMap(Map map)
    {
        await _jsObjectRef.InvokeAsync(
            "setMap",
            map);

    }
}