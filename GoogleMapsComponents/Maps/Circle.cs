using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A circle on the Earth's surface; also known as a "spherical cap".
/// </summary>
public class Circle : ListableEntityBase<CircleOptions>
{
    /// <summary>
    /// Create a circle using the passed CircleOptions, which specify the center, radius, and style.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts"></param>
    public static async Task<Circle> CreateAsync(IJSRuntime jsRuntime, CircleOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Circle", opts);
        var obj = new Circle(jsObjectRef);
        return obj;
    }

    internal Circle(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Gets the LatLngBounds of this Circle.
    /// </summary>
    /// <returns></returns>
    public Task<LatLngBoundsLiteral> GetBounds()
    {
        return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("getBounds");
    }

    /// <summary>
    /// Returns the center of this circle.
    /// </summary>
    /// <returns></returns>
    public Task<LatLngLiteral> GetCenter()
    {
        return _jsObjectRef.InvokeAsync<LatLngLiteral>("getCenter");
    }

    /// <summary>
    /// Returns whether this circle can be dragged by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>("getDraggable");
    }

    /// <summary>
    /// Returns whether this circle can be edited by the user.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetEditable()
    {
        return _jsObjectRef.InvokeAsync<bool>("getEditable");
    }

    /// <summary>
    /// Returns the radius of this circle (in meters).
    /// </summary>
    /// <returns></returns>
    public Task<double> GetRadius()
    {
        return _jsObjectRef.InvokeAsync<double>("getRadius");
    }

    /// <summary>
    /// Returns whether this circle is visible on the map.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>("getVisible");
    }

    /// <summary>
    /// Sets the center of this circle.
    /// </summary>
    /// <param name="center"></param>
    public Task SetCenter(LatLngLiteral center)
    {
        return _jsObjectRef.InvokeAsync("setCenter", center);
    }

    /// <summary>
    /// If set to true, the user can drag this circle over the map.
    /// </summary>
    /// <param name="draggable"></param>
    public Task SetDraggable(bool draggable)
    {
        return _jsObjectRef.InvokeAsync("setDraggable", draggable);
    }

    /// <summary>
    /// If set to true, the user can edit this circle by dragging the control points shown at the center and around the circumference of the circle.
    /// </summary>
    /// <param name="editable"></param>
    public Task SetEditable(bool editable)
    {
        return _jsObjectRef.InvokeAsync("setEditable", editable);
    }

    public Task SetOptions(CircleOptions options)
    {
        return _jsObjectRef.InvokeAsync("setOptions", options);
    }

    /// <summary>
    /// Sets the radius of this circle (in meters).
    /// </summary>
    /// <param name="radius"></param>
    public Task SetRadius(double radius)
    {
        return _jsObjectRef.InvokeAsync("setRadius", radius);
    }

    /// <summary>
    /// Sets the radius of this circle (in meters).
    /// </summary>
    /// <param name="radius"></param>
    public Task SetRadius(decimal radius)
    {
        return SetRadius(Convert.ToDouble(radius));
    }

    /// <summary>
    /// Hides this circle if set to false.
    /// </summary>
    /// <param name="visible"></param>
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync("setVisible", visible);
    }
}