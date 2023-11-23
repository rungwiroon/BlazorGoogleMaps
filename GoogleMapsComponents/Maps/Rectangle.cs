using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A rectangle overlay.
/// </summary>
public class Rectangle : EventEntityBase, IDisposable
{
    public Guid Guid => _jsObjectRef.Guid;

    private Map? _map;

    /// <summary>
    /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
    /// </summary>
    /// <param name="opts"></param>
    public static async Task<Rectangle> CreateAsync(IJSRuntime jsRuntime, RectangleOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Rectangle", opts);

        var obj = new Rectangle(jsObjectRef, opts);

        return obj;
    }

    /// <summary>
    /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
    /// </summary>
    /// <param name="opts"></param>
    internal Rectangle(JsObjectRef jsObjectRef, RectangleOptions? opts = null) : base(jsObjectRef)
    {
        _map = opts?.Map;
    }

    /// <summary>
    /// Returns the bounds of this rectangle.
    /// </summary>
    /// <returns></returns>
    public Task<LatLngBoundsLiteral> GetBounds()
    {
        return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>(
            "getBounds");
    }

    /// <summary>
    /// Returns whether this rectangle can be dragged by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getDraggable");
    }

    /// <summary>
    /// Returns whether this rectangle can be edited by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetEditable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getEditable");
    }

    /// <summary>
    /// Returns the map on which this rectangle is displayed.
    /// </summary>
    /// <returns></returns>
    public Map GetMap()
    {
        return _map;
    }

    /// <summary>
    /// Returns whether this rectangle is visible on the map.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getVisible");
    }

    /// <summary>
    /// Sets the bounds of this rectangle.
    /// </summary>
    /// <param name="bounds"></param>
    public Task SetBounds(LatLngBoundsLiteral bounds)
    {
        return _jsObjectRef.InvokeAsync(
            "setBounds",
            bounds);
    }

    /// <summary>
    /// If set to true, the user can drag this rectangle over the map.
    /// </summary>
    /// <param name="draggable"></param>
    public Task SetDraggable(bool draggable)
    {
        return _jsObjectRef.InvokeAsync(
            "setDraggable",
            draggable);
    }

    /// <summary>
    /// If set to true, the user can edit this rectangle by dragging the control points shown at the corners and on each edge.
    /// </summary>
    /// <param name="editable"></param>
    public Task SetEditable(bool editable)
    {
        return _jsObjectRef.InvokeAsync(
            "setEditable",
            editable);
    }

    /// <summary>
    /// Renders the rectangle on the specified map. If map is set to null, the rectangle will be removed.
    /// </summary>
    /// <param name="map"></param>
    public Task SetMap(Map map)
    {
        _map = map;

        return _jsObjectRef.InvokeAsync(
            "setMap",
            map);
    }

    public Task SetOptions(RectangleOptions options)
    {
        return _jsObjectRef.InvokeAsync(
            "setOptions",
            options);
    }

    /// <summary>
    /// Hides this rectangle if set to false.
    /// </summary>
    /// <param name="visible"></param>
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }
}