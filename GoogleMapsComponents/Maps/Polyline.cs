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
    public class Polyline : JsObjectRef
    {
        private MapComponent _map;

        /// <summary>
        /// Create a polyline using the passed PolylineOptions, which specify both the path of the polyline and the stroke style to use when drawing the polyline.
        /// </summary>
        public Polyline(IJSRuntime jsRuntime, PolylineOptions opts = null)
            : base(jsRuntime)
        {
            if (opts != null)
            {
                _map = opts.Map;

                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapPolylineJsFunctions.init",
                    _guid.ToString(),
                    opts);
            }
            else
            {
                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapPolylineJsFunctions.init",
                    _guid.ToString());
            }
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                "googleMapPolylineJsFunctions.dispose",
                _guid.ToString());
        }

        /// <summary>
        /// Returns whether this shape can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetDraggable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
                "getDraggable");
        }

        /// <summary>
        /// Returns whether this shape can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetEditable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
                "getEditable");
        }

        /// <summary>
        /// Returns the map on which this shape is attached.
        /// </summary>
        /// <returns></returns>
        public MapComponent GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Retrieves the path.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<LatLngLiteral>> GetPath()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<IEnumerable<LatLngLiteral>>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
                "getPath");
        }

        /// <summary>
        /// Returns whether this poly is visible on the map.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetVisible()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
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
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
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
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders this shape on the specified map. 
        /// If map is set to null, the shape will be removed.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task SetMap(MapComponent map)
        {
            _map = map;

            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.setMap",
                _guid.ToString(),
                map?.DivId);
        }

        public Task SetOptions(PolylineOptions options)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
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
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
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
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolylineJsFunctions.invoke",
                _guid.ToString(),
                "setVisible",
                visible);
        }
    }
}
