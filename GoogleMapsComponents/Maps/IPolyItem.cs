using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// IPolyItem is an interface that represents a <see cref="Polyline"/> or <see cref="Polygon"/> item in the Google Maps API.
/// </summary>
public interface IPolyItem : IPoly
{

    /// <summary>
    /// Creates an event listener for the vertexes of the polygon or polyline. The eventName can be one of the following:
    /// 'set_at', 'insert_at' or 'remove_at'.
    /// </summary>
    /// <param name="eventName">Event name to include.</param>
    /// <param name="handler">Handler method</param>
    /// <returns><see cref="Task"/> completion supplying the <see cref="MapEventListener"/> created for this event.</returns>
    public Task<MapEventListener> AddPolyListener(string eventName, Action handler);

    /// <summary>
    /// Creates an event listener for the vertexes of the polygon or polyline. The eventName can be one of the following:
    /// 'set_at', 'insert_at' or 'remove_at'.
    /// </summary>
    /// <typeparam name="T">Event data supplied when the event is invoked.</typeparam>
    /// <param name="eventName">Event name to include.</param>
    /// <param name="handler">Handler method</param>
    /// <returns><see cref="Task"/> completion supplying the <see cref="MapEventListener"/> created for this event.</returns>
    public Task<MapEventListener> AddPolyListener<T>(string eventName, Action<T> handler) where T : EventArgs;

    /// <summary>
    /// Returns whether this shape can be dragged by the user.
    /// </summary>
    /// <returns>JavaScript call completion returning a <see cref="Boolean"/> whether the item is draggable.</returns>
    public Task<bool> GetDraggable();

    /// <summary>
    /// Returns whether this shape can be edited by the user.
    /// </summary>
    /// <returns>JavaScript call completion returning a <see cref="Boolean"/> whether the item is editable.</returns>
    public Task<bool> GetEditable();

    /// <summary>
    /// Retrieves the first path.
    /// </summary>
    /// <returns>JavaScript call completion showcasing the positions of the vertexes.</returns>
    public Task<IEnumerable<LatLngLiteral>> GetPath();

    /// <summary>
    /// Returns whether this poly is visible on the map.
    /// </summary>
    /// <returns><see cref="Task"/> completion with a <see cref="Boolean"/> result whether the item is currently visible.</returns>
    public Task<bool> GetVisible();


    /// <summary>
    /// If set to true, the user can drag this shape over the map. 
    /// The geodesic property defines the mode of dragging.
    /// </summary>
    /// <param name="draggable">If <see langword="true" />, the user will be able to drag the item across the map</param>
    /// <returns>JavaScript method completion.</returns>
    public Task SetDraggable(bool draggable);

    /// <summary>
    /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
    /// </summary>
    /// <param name="editable">If <see langword="true" />, the user will be able to edit the item.</param>
    /// <returns>JavaScript method completion.</returns>
    public Task SetEditable(bool editable);

    /// <summary>
    /// Sets the path.
    /// </summary>
    /// <param name="path">New vertexes to apply to the element</param>
    /// <returns>JavaScript call completion</returns>
    public Task SetPath(IEnumerable<LatLngLiteral> path);

    /// <summary>
    /// Hides this poly if set to false.
    /// </summary>
    /// <param name="visible">Whether to make the item visible</param>
    /// <returns>JavaScript call completion</returns>
    public Task SetVisible(bool visible);
}
