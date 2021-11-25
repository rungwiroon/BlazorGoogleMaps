using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A polyline is a linear overlay of connected line segments on the map.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class Polyline : MVCObject //ListableEntityBase<PolylineOptions>
    {
        /// <summary>
        /// Create a polyline using the passed PolylineOptions, which specify both the path of the polyline and the stroke style to use when drawing the polyline.
        /// </summary>
        public async static Task<Polyline> CreateAsync(IJSRuntime jsRuntime, PolylineOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.Polyline",
                opts);

            return new Polyline(jsObjectRef);
        }

        internal Polyline(IJSObjectReference jsObjectRef)
            :base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns whether this shape can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetDraggable()
        {
            return this.InvokeAsync<bool>(
                "getDraggable");
        }

        /// <summary>
        /// Returns whether this shape can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetEditable()
        {
            return this.InvokeAsync<bool>(
                "getEditable");
        }

        /// <summary>
        /// Retrieves the path.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<LatLngLiteral>> GetPath()
        {
            return this.InvokeAsync<IEnumerable<LatLngLiteral>>(
                "getPath");
        }

        /// <summary>
        /// Returns whether this poly is visible on the map.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetVisible()
        {
            return this.InvokeAsync<bool>(
                "getVisible");
        }

        /// <summary>
        /// If set to true, the user can drag this shape over the map. 
        /// The geodesic property defines the mode of dragging.
        /// </summary>
        /// <param name="draggable"></param>
        /// <returns></returns>
        public ValueTask SetDraggable(bool draggable)
        {
            return this.InvokeVoidAsync(
                "setDraggable",
                draggable);
        }

        /// <summary>
        /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
        /// </summary>
        /// <param name="editable"></param>
        /// <returns></returns>
        public ValueTask SetEditable(bool editable)
        {
            return this.InvokeVoidAsync(
                "setEditable",
                editable);
        }

        public ValueTask SetOptions(PolylineOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }

        /// <summary>
        /// Sets the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ValueTask SetPath(IEnumerable<LatLngLiteral> path)
        {
            return this.InvokeVoidAsync(
                "setPath",
                path);
        }

        /// <summary>
        /// Hides this poly if set to false.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        public ValueTask SetVisible(bool visible)
        {
            return this.InvokeVoidAsync(
                "setVisible",
                visible);
        }
    }
}
