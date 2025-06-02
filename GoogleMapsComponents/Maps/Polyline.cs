using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A polyline is a linear overlay of connected line segments on the map.
/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polyline
/// </summary>
public class Polyline : ListableEntityBase<PolylineOptions>, IPolyItem
{

    /// <summary>
    /// Create a polyline using the passed PolylineOptions, which specify both the path of the polyline and the stroke style to use when drawing the polyline.
    /// </summary>
    public static async Task<Polyline> CreateAsync(IJSRuntime jsRuntime, PolylineOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Polyline", opts);

        var obj = new Polyline(jsObjectRef);

        return obj;
    }

    /// <summary>
    /// Constructor for use in ListableEntityListBase. Must be the first constructor!
    /// </summary>
    internal Polyline(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }


    /// <summary>
    /// Creates an event listener for the vertexes of the polygon. The eventName can be one of the following:
    /// 'set_at', 'insert_at' or 'remove_at'.
    /// </summary>
    /// <param name="eventName">Event name to include.</param>
    /// <param name="handler">Handler method</param>
    /// <returns><see cref="Task"/> completion supplying the <see cref="MapEventListener"/> created for this event.</returns>
    public async Task<MapEventListener> AddPolyListener(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addPolyListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }


    /// <summary>
    /// Creates an event listener for the vertexes of the polygon. The eventName can be one of the following:
    /// 'set_at', 'insert_at' or 'remove_at'.
    /// </summary>
    /// <typeparam name="T">Event data supplied when the event is invoked.</typeparam>
    /// <param name="eventName">Event name to include.</param>
    /// <param name="handler">Handler method</param>
    /// <returns><see cref="Task"/> completion supplying the <see cref="MapEventListener"/> created for this event.</returns>
    public async Task<MapEventListener> AddPolyListener<T>(string eventName, Action<T> handler) where T : EventArgs
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addPolyListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }

    /// <inheritdoc />
    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>("getDraggable");
    }

    /// <inheritdoc />
    public Task<bool> GetEditable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getEditable");
    }

    /// <inheritdoc />
    public Task<IEnumerable<LatLngLiteral>> GetPath()
    {
        return _jsObjectRef.InvokeAsync<IEnumerable<LatLngLiteral>>("getPath");
    }

    /// <summary>
    /// Creates paths lime MVCArray where elements could be appended
    /// </summary>
    /// <returns></returns>
    public async Task<PolylinePath> CreatePath()
    {
        var id = Guid.NewGuid();
        await _jsObjectRef.InvokeAsync("createPath", id.ToString());
        var path = new PolylinePath(new JsObjectRef(_jsObjectRef.JSRuntime, id));

        return path;
    }
    

    /// <inheritdoc />
    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getVisible");
    }

    /// <inheritdoc />
    public Task SetDraggable(bool draggable)
    {
        return _jsObjectRef.InvokeAsync(
            "setDraggable",
            draggable);
    }

    /// <summary>
    /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
    /// </summary>
    /// <param name="editable"></param>
    /// <returns></returns>
    public Task SetEditable(bool editable)
    {
        return _jsObjectRef.InvokeAsync(
            "setEditable",
            editable);
    }

    public Task SetOptions(PolylineOptions options)
    {
        return _jsObjectRef.InvokeAsync(
            "setOptions",
            options);
    }

    /// <inheritdoc />
    public Task SetPath(IEnumerable<LatLngLiteral> path)
    {
        return _jsObjectRef.InvokeAsync(
            "setPath",
            path);
    }

    /// <inheritdoc />
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }

}