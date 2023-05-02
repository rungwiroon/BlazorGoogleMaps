using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

public class EventEntityBase
{
    private readonly JsObjectRef _jsObjectRef;

    protected EventEntityBase(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }
    //Note: Maybe we want to keep track of events, and make sure that no duplicates are added, or maybe that is up to the user?
    public async Task<MapEventListener> AddListener(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        return new MapEventListener(listenerRef);
    }

    public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        return new MapEventListener(listenerRef);
    }
    
    //Note: Might want to wrap the handler with our own handler to make sure that we dispose the event after trigger?
    public async Task<MapEventListener> AddListenerOnce(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        return new MapEventListener(listenerRef);
    }

    public async Task<MapEventListener> AddListenerOnce<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        return new MapEventListener(listenerRef);
    }
}