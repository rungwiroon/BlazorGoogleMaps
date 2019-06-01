using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Polygon : JsObjectRef
    {
        private MapComponent _map;

        /// <summary>
        /// Create a polygon using the passed PolygonOptions, which specify the polygon's path, the stroke style for the polygon's edges, and the fill style for the polygon's interior regions. 
        /// A polygon may contain one or more paths, where each path consists of an array of LatLngs.
        /// </summary>
        /// <param name="opts"></param>
        public Polygon(IJSRuntime jsRuntime, PolygonOptions opts = null)
            : base(jsRuntime)
        {
            if (opts != null)
            {
                _map = opts.Map;

                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapPolygonJsFunctions.init",
                    _guid.ToString(),
                    opts);
            }
            else
            {
                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapPolygonJsFunctions.init",
                    _guid.ToString());
            }
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                "googleMapPolygonJsFunctions.dispose",
                _guid.ToString());
        }

        /// <summary>
        /// Returns whether this shape can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetDraggble()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "getDraggble");
        }

        /// <summary>
        /// Returns whether this shape can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetEditable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
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
        /// Retrieves the first path.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<LatLngLiteral>> GetPath()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<IEnumerable<LatLngLiteral>>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "getPath");
        }

        /// <summary>
        /// Retrieves the paths for this polygon.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<IEnumerable<LatLngLiteral>>> GetPaths()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<IEnumerable<IEnumerable<LatLngLiteral>>>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "getPaths");
        }

        /// <summary>
        /// Returns whether this poly is visible on the map.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetVisible()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "getVisible");
        }

        /// <summary>
        /// If set to true, the user can drag this shape over the map. 
        /// The geodesic property defines the mode of dragging.
        /// </summary>
        /// <param name="draggble"></param>
        public Task SetDraggble(bool draggble)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setDraggble",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this shape by dragging the control points shown at the vertices and on each segment.
        /// </summary>
        /// <param name="editable"></param>
        public Task SetEditable(bool editable)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders this shape on the specified map. If map is set to null, the shape will be removed.
        /// </summary>
        /// <param name="map"></param>
        public Task SetMap(MapComponent map)
        {
            _map = map;

            return _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                "googleMapPolygonJsFunctions.setMap",
                _guid.ToString(),
                map?.DivId);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="options"></param>
        public Task SetOptions(PolygonOptions options)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setOptions",
                options);
        }

        /// <summary>
        /// Sets the first path. See PolygonOptions for more details.
        /// </summary>
        /// <param name="path"></param>
        public Task SetPath(IEnumerable<LatLngLiteral> path)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setPath",
                path);
        }

        /// <summary>
        /// Sets the path for this polygon.
        /// </summary>
        /// <param name="paths"></param>
        public Task SetPaths(IEnumerable<IEnumerable<LatLngLiteral>> paths)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setPaths",
                paths);
        }

        /// <summary>
        /// Hides this poly if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public Task SetVisible(bool visible)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapPolygonJsFunctions.invoke",
                _guid.ToString(),
                "setVisible",
                visible);
        }
    }
}
