using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class Polygon : MVCObject
    {
        /// <summary>
        /// Create a polygon using the passed PolygonOptions, which specify the polygon's path, the stroke style for the polygon's edges, and the fill style for the polygon's interior regions. 
        /// A polygon may contain one or more paths, where each path consists of an array of LatLngs.
        /// </summary>
        /// <param name="opts"></param>
        public static async ValueTask<Polygon> CreateAsync(IJSRuntime jsRuntime, PolygonOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.Polygon",
                opts);

            var obj = new Polygon(jsObjectRef);

            return obj;
        }

        internal Polygon(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns whether this shape can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetDraggble()
        {
            return this.InvokeAsync<bool>(
                "getDraggble");
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
        /// Returns the map on which this shape is attached.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<Map?> GetMap()
        {
            var mapRef = await this.InvokeAsync<IJSObjectReference>(
                "getMap");
            return new Map(mapRef);
        }

        /// <summary>
        /// Retrieves the first path.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<LatLngLiteral>> GetPath()
        {
            return this.InvokeAsync<IEnumerable<LatLngLiteral>>(
                "getPath");
        }

        /// <summary>
        /// Retrieves the paths for this polygon.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<IEnumerable<LatLngLiteral>>> GetPaths()
        {
            return this.InvokeAsync<IEnumerable<IEnumerable<LatLngLiteral>>>(
                "getPaths");
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
        /// <param name="draggble"></param>
        public ValueTask SetDraggble(bool draggble)
        {
            return this.InvokeVoidAsync(
                "setDraggble",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
        /// </summary>
        /// <param name="editable"></param>
        public ValueTask SetEditable(bool editable)
        {
            return this.InvokeVoidAsync(
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders this shape on the specified map. If map is set to null, the shape will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map? map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                map?.Reference);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="options"></param>
        public ValueTask SetOptions(PolygonOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="path"></param>
        public ValueTask SetPath(IEnumerable<LatLngLiteral> path)
        {
            return this.InvokeVoidAsync(
                "setPath",
                path);
        }

        /// <summary>
        /// Sets the path for this polygon.
        /// </summary>
        /// <param name="paths"></param>
        public ValueTask SetPaths(IEnumerable<IEnumerable<LatLngLiteral>> paths)
        {
            return this.InvokeVoidAsync(
                "setPaths",
                paths);
        }

        /// <summary>
        /// Hides this poly if set to false.
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
