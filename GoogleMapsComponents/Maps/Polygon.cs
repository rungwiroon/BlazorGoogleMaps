using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Polygon : IAsyncDisposable
    {
        protected readonly JsObjectRef _jsObjectRef;
        private Map _map;
        
        public Guid Guid => _jsObjectRef.Guid;

        /// <summary>
        /// Create a polygon using the passed PolygonOptions, which specify the polygon's path, the stroke style for the polygon's edges, and the fill style for the polygon's interior regions. 
        /// A polygon may contain one or more paths, where each path consists of an array of LatLngs.
        /// </summary>
        /// <param name="opts"></param>
        public static async Task<Polygon> CreateAsync(IJSRuntime jsRuntime, PolygonOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Polygon", opts);

            var obj = new Polygon(jsObjectRef, opts);

            return obj;
        }

        /// <summary>
        /// Create a polygon using the passed PolygonOptions, which specify the polygon's path, the stroke style for the polygon's edges, and the fill style for the polygon's interior regions. 
        /// A polygon may contain one or more paths, where each path consists of an array of LatLngs.
        /// </summary>
        /// <param name="opts"></param>
        internal Polygon(JsObjectRef jsObjectRef, PolygonOptions opts = null)
        {
            _jsObjectRef = jsObjectRef;
            _map = opts?.Map;
        }

        public ValueTask DisposeAsync()
        {
            return _jsObjectRef.DisposeAsync();
        }

        /// <summary>
        /// Returns whether this shape can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetDraggble()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getDraggble");
        }

        /// <summary>
        /// Returns whether this shape can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetEditable()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getEditable");
        }

        /// <summary>
        /// Returns the map on which this shape is attached.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Retrieves the first path.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<LatLngLiteral>> GetPath()
        {
            return _jsObjectRef.InvokeAsync<IEnumerable<LatLngLiteral>>(
                "getPath");
        }

        /// <summary>
        /// Retrieves the paths for this polygon.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<IEnumerable<LatLngLiteral>>> GetPaths()
        {
            return _jsObjectRef.InvokeAsync<IEnumerable<IEnumerable<LatLngLiteral>>>(
                "getPaths");
        }

        /// <summary>
        /// Returns whether this poly is visible on the map.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetVisible()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getVisible");
        }

        /// <summary>
        /// If set to true, the user can drag this shape over the map. 
        /// The geodesic property defines the mode of dragging.
        /// </summary>
        /// <param name="draggble"></param>
        public ValueTask SetDraggble(bool draggble)
        {
            return _jsObjectRef.InvokeAsync(
                "setDraggble",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
        /// </summary>
        /// <param name="editable"></param>
        public ValueTask SetEditable(bool editable)
        {
            return _jsObjectRef.InvokeAsync(
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders this shape on the specified map. If map is set to null, the shape will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map map)
        {
            _map = map;

            return _jsObjectRef.InvokeAsync(
                "setMap",
                map);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="options"></param>
        public ValueTask SetOptions(PolygonOptions options)
        {
            return _jsObjectRef.InvokeAsync(
                "setOptions",
                options);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="path"></param>
        public ValueTask SetPath(IEnumerable<LatLngLiteral> path)
        {
            return _jsObjectRef.InvokeAsync(
                "setPath",
                path);
        }

        /// <summary>
        /// Sets the path for this polygon.
        /// </summary>
        /// <param name="paths"></param>
        public ValueTask SetPaths(IEnumerable<IEnumerable<LatLngLiteral>> paths)
        {
            return _jsObjectRef.InvokeAsync(
                "setPaths",
                paths);
        }

        /// <summary>
        /// Hides this poly if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public ValueTask SetVisible(bool visible)
        {
            return _jsObjectRef.InvokeAsync(
                "setVisible",
                visible);
        }

        public ValueTask InvokeAsync(string functionName, params object[] args)
        {
            return _jsObjectRef.InvokeAsync(functionName, args);
        }

        public async ValueTask<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async ValueTask<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }
    }
}
