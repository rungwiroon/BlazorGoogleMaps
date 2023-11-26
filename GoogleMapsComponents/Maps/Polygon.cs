using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A polygon (like a polyline) defines a series of connected coordinates in an ordered sequence. Additionally, polygons form a closed loop and define a filled region
/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polygon
/// </summary>
public class Polygon : ListableEntityBase<PolygonOptions>
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
    /// Retrieves the first path.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Sets the first path. See PolygonOptions for more details.
    /// </summary>
    /// <param name="path"></param>
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

    /// <summary>
    /// Hides this poly if set to false.
    /// </summary>
    /// <param name="visible"></param>
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }
}