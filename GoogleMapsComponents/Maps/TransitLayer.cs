using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// https://developers.google.com/maps/documentation/javascript/reference/map#TransitLayer
/// </summary>
public class TransitLayer : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;

    public static async Task<TransitLayer> CreateAsync(IJSRuntime jsRuntime)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.TransitLayer");
        var obj = new TransitLayer(jsObjectRef);

        return obj;
    }

    internal TransitLayer(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    public virtual Task<Map> GetMap()
    {
        return _jsObjectRef.InvokeAsync<Map>("getMap");
    }

    public virtual async Task SetMap(Map? map)
    {
        await _jsObjectRef.InvokeAsync("setMap", map);
    }
}