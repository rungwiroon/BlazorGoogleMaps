using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A polygon (like a polyline) defines a series of connected coordinates in an ordered sequence. Additionally, polygons form a closed loop and define a filled region
/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polygon
/// </summary>
public class Polygon : ListableEntityBase<PolygonOptions>, IPolyItem
{

    /// <summary>
    /// Create a polygon using the passed PolygonOptions, which specify the polygon's path, the stroke style for the polygon's edges, and the fill style for the polygon's interior regions. 
    /// A polygon may contain one or more paths, where each path consists of an array of LatLngs.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts"></param>
    public static async Task<Polygon> CreateAsync(IJSRuntime jsRuntime, PolygonOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Polygon", opts);

        var obj = new Polygon(jsObjectRef);

        return obj;
    }

    /// <summary>
    /// Constructor for use in ListableEntityListBase. Must be the first constructor!
    /// </summary>
    internal Polygon(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }
     
    /// <inheritdoc />
    public async Task<MapEventListener> AddPolyListener(string eventName, Action handler)
    {
        var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addPolyListener", eventName, handler);

        var eventListener = new MapEventListener(listenerRef);
        AddEvent(eventName, eventListener);
        return eventListener;
    }
    
    /// <inheritdoc />
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
        return _jsObjectRef.InvokeAsync<bool>(
            "getDraggable");
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
        return _jsObjectRef.InvokeAsync<IEnumerable<LatLngLiteral>>(
            "getPath");
    }

    /// <summary>
    /// Retrieves the paths for this polygon.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<IEnumerable<LatLngLiteral>>> GetPaths()
    {
        return _jsObjectRef.InvokeAsync<IEnumerable<IEnumerable<LatLngLiteral>>>(
            "getPaths");
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

    /// <inheritdoc />
    public Task SetEditable(bool editable)
    {
        return _jsObjectRef.InvokeAsync(
            "setEditable",
            editable);
    }

    /// <summary>
    /// Sets the first path. See PolygonOptions for more details.
    /// </summary>
    /// <param name="options"></param>
    public Task SetOptions(PolygonOptions options)
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

    /// <summary>
    /// Sets the path for this polygon.
    /// </summary>
    /// <param name="paths"></param>
    public Task SetPaths(IEnumerable<IEnumerable<LatLngLiteral>> paths)
    {
        return _jsObjectRef.InvokeAsync(
            "setPaths",
            paths);
    }

    /// <inheritdoc />
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }
}