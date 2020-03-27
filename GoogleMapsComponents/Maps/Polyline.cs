using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A polyline is a linear overlay of connected line segments on the map.
    /// </summary>
    public class Polyline : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;
        private Map _map;

        /// <summary>
        /// Access polyline using guid and window._blazorGoogleMapsObjects[GUID_STRING]
        /// </summary>
        public Guid Guid => _jsObjectRef.Guid;

        /// <summary>
        /// Create a polyline using the passed PolylineOptions, which specify both the path of the polyline and the stroke style to use when drawing the polyline.
        /// </summary>
        public async static Task<Polyline> CreateAsync(IJSRuntime jsRuntime, PolylineOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Polyline", opts);

            var obj = new Polyline(jsObjectRef, opts);

            return obj;
        }

        /// <summary>
        /// Create a polyline using the passed PolylineOptions, which specify both the path of the polyline and the stroke style to use when drawing the polyline.
        /// </summary>
        private Polyline(JsObjectRef jsObjectRef, PolylineOptions opts = null)
        {
            _jsObjectRef = jsObjectRef;
            _map = opts?.Map;
        }

        public void Dispose()
        {
            _jsObjectRef.Dispose();
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
        /// Returns the map on which this shape is attached.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return _map;
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

        /// <summary>
        /// Renders this shape on the specified map. 
        /// If map is set to null, the shape will be removed.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task SetMap(Map map)
        {
            _map = map;

            return _jsObjectRef.InvokeAsync(
                "setMap",
                map);
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

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }
    }
}
