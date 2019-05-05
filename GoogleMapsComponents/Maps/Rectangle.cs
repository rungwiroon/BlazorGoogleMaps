using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A rectangle overlay.
    /// </summary>
    public class Rectangle : JsObjectRef
    {
        private MapComponent _map;

        /// <summary>
        /// Create a rectangle using the passed RectangleOptions, which specify the bounds and style.
        /// </summary>
        /// <param name="opts"></param>
        public Rectangle(IJSRuntime jsRuntime, RectangleOptions opts = null)
            : base(jsRuntime)
        {
            if (opts != null)
            {
                _map = opts.Map;

                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapRectangleJsFunctions.init",
                    _guid.ToString(),
                    opts);
            }
            else
            {
                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapRectangleJsFunctions.init",
                    _guid.ToString());
            }
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapRectangleJsFunctions.dispose",
                    _guid.ToString());
        }

        /// <summary>
        /// Returns the bounds of this rectangle.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngBoundsLiteral> GetBounds()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<LatLngBoundsLiteral>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "getBounds");
        }

        /// <summary>
        /// Returns whether this rectangle can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetDraggable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "getDraggable");
        }

        /// <summary>
        /// Returns whether this rectangle can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetEditable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "getEditable");
        }

        /// <summary>
        /// Returns the map on which this rectangle is displayed.
        /// </summary>
        /// <returns></returns>
        public MapComponent GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Returns whether this rectangle is visible on the map.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetVisible()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "getVisible");
        }

        /// <summary>
        /// Sets the bounds of this rectangle.
        /// </summary>
        /// <param name="bounds"></param>
        public Task SetBounds(LatLngBoundsLiteral bounds)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "setBounds",
                bounds);
        }

        /// <summary>
        /// If set to true, the user can drag this rectangle over the map.
        /// </summary>
        /// <param name="draggble"></param>
        public Task SetDraggable(bool draggble)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "setDraggable",
                draggble);
        }

        /// <summary>
        /// If set to true, the user can edit this rectangle by dragging the control points shown at the corners and on each edge.
        /// </summary>
        /// <param name="editable"></param>
        public Task SetEditable(bool editable)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders the rectangle on the specified map. If map is set to null, the rectangle will be removed.
        /// </summary>
        /// <param name="map"></param>
        public Task SetMap(MapComponent map)
        {
            _map = map;

            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.setMap",
                _guid.ToString(),
                map.DivId);
        }

        public Task SetOptions(RectangleOptions options)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "setOptions",
                options);
        }

        /// <summary>
        /// Hides this rectangle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public Task SetVisible(bool visible)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapRectangleJsFunctions.invoke",
                _guid.ToString(),
                "setVisible",
                visible);
        }
    }
}
