using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

[Obsolete("Use addListener instead, see: https://developers.google.com/maps/deprecations?hl=en#googlemapseventadddomlistener_and_googlemapseventadddomlisteneronce_deprecated_on_april_7_2022")]
public class Event
{
    private readonly IJSRuntime _jsRuntime;

    public Event(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Cross browser event handler registration. 
    /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
    /// </summary>
    public Task AddDomListener(object instance, string eventName, Action handler, bool? capture)
    {
        return _jsRuntime.MyInvokeAsync(
            "google.maps.event.addDomListener", instance, eventName, handler, capture);
    }

    /// <summary>
    /// Cross browser event handler registration. 
    /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
    /// </summary>
    public Task AddDomListener<T>(object instance, string eventName, Action<T> handler, bool? capture)
    {
        return _jsRuntime.MyInvokeAsync(
            "google.maps.event.addDomListener", instance, eventName, handler, capture);
    }
}