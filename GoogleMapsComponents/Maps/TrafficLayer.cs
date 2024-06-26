using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// https://developers.google.com/maps/documentation/javascript/reference/map#TrafficLayer
/// </summary>
public class TrafficLayer : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;
    public static async Task<TrafficLayer> CreateAsync(IJSRuntime jsRuntime, TrafficLayerOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.TrafficLayer", opts);

        var obj = new TrafficLayer(jsObjectRef);

        return obj;
    }

    /// <summary>
    /// Constructor for use in ListableEntityListBase. Must be the first constructor!
    /// </summary>
    internal TrafficLayer(JsObjectRef jsObjectRef)
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