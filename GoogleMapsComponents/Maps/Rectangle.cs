using Microsoft.JSInterop;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A rectangle overlay.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class Rectangle : MVCObject
    {
        /// <summary>
        /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
        /// </summary>
        /// <param name="opts"></param>
        public static async Task<Rectangle> CreateAsync(IJSRuntime jsRuntime, RectangleOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.Rectangle",
                opts);
            var obj = new Rectangle(jsObjectRef);
            return obj;
        }

        internal Rectangle(IJSObjectReference jsObjectRef)
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
        public ValueTask<Map?> GetMap()
        {
            return this.InvokeWithReturnedObjectRefAsync(
                "getMap",
                objRef => new Map(objRef));
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
            return this.InvokeVoidAsync(
                "setBounds",
                bounds);
        }

        /// <summary>
        /// If set to true, the user can drag this rectangle over the map.
        /// </summary>
        /// <param name="draggble"></param>
        public ValueTask SetDraggable(bool draggble)
        {
            return this.InvokeVoidAsync(
                "setDraggable",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this rectangle by dragging the control points shown at the corners and on each edge.
        /// </summary>
        /// <param name="editable"></param>
        public ValueTask SetEditable(bool editable)
        {
            return this.InvokeVoidAsync(
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders the rectangle on the specified map. If map is set to null, the rectangle will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map? map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                map);
        }

        public ValueTask SetOptions(RectangleOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }

        /// <summary>
        /// Hides this rectangle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public ValueTask SetVisible(bool visible)
        {
            return this.InvokeVoidAsync(
                "setVisible",
                visible);
        }
    }
}
