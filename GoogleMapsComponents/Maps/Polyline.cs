using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A polyline is a linear overlay of connected line segments on the map.
/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polyline
/// </summary>
public class Polyline : ListableEntityBase<PolylineOptions>
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
    /// Returns whether this shape can be dragged by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getDraggable");
    }

    /// <summary>
    /// Returns whether this shape can be edited by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetEditable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getEditable");
    }

    /// <summary>
    /// Retrieves the path.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Returns whether this poly is visible on the map.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getVisible");
    }

    /// <summary>
    /// If set to true, the user can drag this shape over the map. 
    /// The geodesic property defines the mode of dragging.
    /// </summary>
    /// <param name="draggable"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Sets the path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Task SetPath(IEnumerable<LatLngLiteral> path)
    {
        return _jsObjectRef.InvokeAsync(
            "setPath",
            path);
    }

    /// <summary>
    /// Hides this poly if set to false.
    /// </summary>
    /// <param name="visible"></param>
    /// <returns></returns>
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }

}