using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A rectangle overlay.
    /// </summary>
    public class Rectangle : JsObjectRef
    {
        //private readonly JsObjectRef _jsObjetRef;
        //private Map _map;

        /// <summary>
        /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
        /// </summary>
        /// <param name="opts"></param>
        public static async Task<Rectangle> CreateAsync(IJSRuntime jsRuntime, RectangleOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Rectangle", opts);

            //var obj = new Rectangle(jsObjectRef, opts);

            //return obj;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
        /// </summary>
        /// <param name="opts"></param>
        internal Rectangle(IJSObjectReference jsObjectRef, RectangleOptions opts = null)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns the bounds of this rectangle.
        /// </summary>
        /// <returns></returns>
        public ValueTask<LatLngBoundsLiteral> GetBounds()
        {
            return InvokeAsync<LatLngBoundsLiteral>(
                "getBounds");
        }

        /// <summary>
        /// Returns whether this rectangle can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetDraggable()
        {
            return InvokeAsync<bool>(
                "getDraggable");
        }

        /// <summary>
        /// Returns whether this rectangle can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetEditable()
        {
            return InvokeAsync<bool>(
                "getEditable");
        }

        /// <summary>
        /// Returns the map on which this rectangle is displayed.
        /// </summary>
        /// <returns></returns>
        public ValueTask<Map> GetMap()
        {
            return InvokeWithReturnedObjectRefAsync("getMap", objRef => new Map(objRef));
        }

        /// <summary>
        /// Returns whether this rectangle is visible on the map.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetVisible()
        {
            return InvokeAsync<bool>(
                "getVisible");
        }

        /// <summary>
        /// Sets the bounds of this rectangle.
        /// </summary>
        /// <param name="bounds"></param>
        public ValueTask SetBounds(LatLngBoundsLiteral bounds)
        {
            return InvokeVoidAsync(
                "setBounds",
                bounds);
        }

        /// <summary>
        /// If set to true, the user can drag this rectangle over the map.
        /// </summary>
        /// <param name="draggble"></param>
        public ValueTask SetDraggable(bool draggble)
        {
            return InvokeVoidAsync(
                "setDraggable",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this rectangle by dragging the control points shown at the corners and on each edge.
        /// </summary>
        /// <param name="editable"></param>
        public ValueTask SetEditable(bool editable)
        {
            return InvokeVoidAsync(
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders the rectangle on the specified map. If map is set to null, the rectangle will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map map)
        {
            return InvokeVoidAsync(
                "setMap",
                map);
        }

        public ValueTask SetOptions(RectangleOptions options)
        {
            return InvokeVoidAsync(
                "setOptions",
                options);
        }

        /// <summary>
        /// Hides this rectangle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public ValueTask SetVisible(bool visible)
        {
            return InvokeVoidAsync(
                "setVisible",
                visible);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }
    }
}
