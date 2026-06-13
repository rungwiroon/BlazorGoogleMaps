using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// Base class for Google Maps objects that support event handling and proper resource cleanup.
/// Provides event listener management and async disposal patterns.
/// </summary>
public abstract class EventEntityBase : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    protected readonly JsObjectRef _jsObjectRef;

    private readonly Dictionary<string, List<MapEventListener>> _eventListeners;

    private bool _isDisposed;

    /// <summary>
    /// Adds an event listener to the internal collection for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="listener">The event listener to add.</param>
    protected void AddEvent(string eventName, MapEventListener listener)
    {
        if (!_eventListeners.TryGetValue(eventName, out var collection))
        {
            collection = new List<MapEventListener>();
            _eventListeners.Add(eventName, collection);
        }
        collection.Add(listener);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventEntityBase"/> class.
    /// </summary>
    /// <param name="jsObjectRef">The JavaScript object reference representing the map entity.</param>
    protected EventEntityBase(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
        _eventListeners = new Dictionary<string, List<MapEventListener>>();
    }

    /// <summary>
    /// Adds an event listener to the map entity for the specified event.
    /// </summary>
    /// <param name="eventName">The name of the event to listen for.</param>
    /// <param name="handler">The callback to invoke when the event fires.</param>
    /// <returns>A <see cref="MapEventListener"/> that can be used to remove the listener.</returns>
    public async Task<MapEventListener> AddListener(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    /// <summary>
    /// Adds an event listener to the map entity for the specified event with a typed argument.
    /// </summary>
    /// <typeparam name="T">The type of the event argument.</typeparam>
    /// <param name="eventName">The name of the event to listen for.</param>
    /// <param name="handler">The callback to invoke when the event fires.</param>
    /// <returns>A <see cref="MapEventListener"/> that can be used to remove the listener.</returns>
    public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    /// <summary>
    /// Adds a one-time event listener to the map entity for the specified event.
    /// The listener will be automatically removed after the event fires once.
    /// </summary>
    /// <remarks>TODO: Consider wrapping the handler to ensure proper disposal after trigger.</remarks>
    /// <param name="eventName">The name of the event to listen for.</param>
    /// <param name="handler">The callback to invoke when the event fires.</param>
    /// <returns>A <see cref="MapEventListener"/> that can be used to remove the listener before it fires.</returns>
    public async Task<MapEventListener> AddListenerOnce(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    /// <summary>
    /// Adds a one-time event listener to the map entity for the specified event with a typed argument.
    /// The listener will be automatically removed after the event fires once.
    /// </summary>
    /// <typeparam name="T">The type of the event argument.</typeparam>
    /// <param name="eventName">The name of the event to listen for.</param>
    /// <param name="handler">The callback to invoke when the event fires.</param>
    /// <returns>A <see cref="MapEventListener"/> that can be used to remove the listener before it fires.</returns>
    public async Task<MapEventListener> AddListenerOnce<T>(string eventName, Action<T> handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
            "addListenerOnce", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    /// <summary>
    /// Removes all event listeners for the specified event name.
    /// </summary>
    /// <remarks>
    /// The event key is preserved in the dictionary with an empty list, maintaining knowledge that
    /// event listeners were previously attached to this event name.
    /// </remarks>
    /// <param name="eventName">The name of the event.</param>
    public async Task ClearListeners(string eventName)
    {
        if (_eventListeners.TryGetValue(eventName, out var listeners))
        {
            foreach (var listener in listeners.Where(listener => !listener.IsRemoved))
            {
                await listener.RemoveAsync();
            }

            _eventListeners[eventName].Clear();
        }
    }


    /// <summary>
    /// Asynchronously releases all resources used by the current instance.
    /// </summary>
    /// <remarks>
    /// This method performs async cleanup, disposes unmanaged resources, and suppresses finalization.
    /// </remarks>
    public virtual async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Performs the core async disposal logic, including cleanup of all event listeners and the JavaScript object reference.
    /// </summary>
    /// <remarks>
    /// This method removes all registered event listeners and disposes the underlying JavaScript object reference.
    /// </remarks>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        foreach (var eventListener in _eventListeners.SelectMany(listener => listener.Value))
        {
            if (eventListener.IsRemoved)
            {
                continue;
            }

            await eventListener.DisposeAsync();
        }

        _eventListeners.Clear();
        await _jsObjectRef.DisposeAsync();
    }

    /// <summary>
    /// Releases unmanaged resources and optionally releases managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _isDisposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the current instance.
    /// </summary>
    public virtual void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}