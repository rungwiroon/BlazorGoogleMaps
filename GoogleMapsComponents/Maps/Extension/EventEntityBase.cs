using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

public abstract class EventEntityBase : IDisposable
{
    protected readonly JsObjectRef _jsObjectRef;
    private readonly Dictionary<string, List<MapEventListener>> EventListeners;
    private bool _isDisposed;

    private void AddEvent(string eventName, MapEventListener listener)
    {
        if (!EventListeners.TryGetValue(eventName, out var collection))
        {
            collection = new List<MapEventListener>();
            EventListeners.Add(eventName, collection);
        }
        collection.Add(listener);
    }

    protected EventEntityBase(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
        EventListeners = new Dictionary<string, List<MapEventListener>>();
    }

    public async Task<MapEventListener> AddListener(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    //Note: Might want to wrap the handler with our own handler to make sure that we dispose the event after trigger?
    public async Task<MapEventListener> AddListenerOnce(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    public async Task<MapEventListener> AddListenerOnce<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    public async Task ClearListeners(string eventName)
    {
        if (EventListeners.TryGetValue(eventName, out var listeners))
        {
            foreach (var listener in listeners.Where(listener => !listener.IsRemoved))
            {
                await listener.RemoveAsync();
            }

            //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
            //so, instead to clear the list and remove the key from dictionary, I prefer to leave the key with an empty list
            EventListeners[eventName].Clear();
            //EventListeners.Remove(eventName);
        }
    }


    public virtual async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// This method takes care of disposing the possible event listeners that were added.
    /// It also dispose the JsObjectRef and uses it to remove listeners
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        foreach (var eventListener in EventListeners.SelectMany(listener => listener.Value))
        {
            if (eventListener.IsRemoved)
            {
                continue;
            }

            await eventListener.DisposeAsync();
        }

        EventListeners.Clear();
        await _jsObjectRef.DisposeAsync();
    }

    protected virtual void Dispose(bool disposing)
    {

        if (!_isDisposed)
        {
            //_jsObjectRef.Dispose();

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }
    }

    public virtual void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}