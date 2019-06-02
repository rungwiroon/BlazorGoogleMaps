using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A circle on the Earth's surface; also known as a "spherical cap".
    /// </summary>
    public class Circle : JsObjectRef
    {
        private MapComponent _map;

        /// <summary>
        /// Create a circle using the passed CircleOptions, which specify the center, radius, and style.
        /// </summary>
        /// <param name="opts"></param>
        public Circle(IJSRuntime jsRuntime, CircleOptions opts = null)
            : base(jsRuntime, "google.maps.Circle", opts)
        {
            if (opts != null)
            {
                _map = opts.Map;

                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapCircleJsFunctions.init",
                    _guid.ToString(),
                    opts);
            }
            else
            {
                _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapCircleJsFunctions.init",
                    _guid.ToString());
            }
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                    "googleMapCircleJsFunctions.dispose",
                    _guid.ToString());
        }

        /// <summary>
        /// Gets the LatLngBounds of this Circle.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngBoundsLiteral> GetBounds()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<LatLngBoundsLiteral>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "getBounds");
        }

        /// <summary>
        /// Returns the center of this circle.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetCenter()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<LatLngLiteral>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "getBounds");
        }

        /// <summary>
        /// Returns whether this circle can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetDraggable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "GetDraggable");
        }

        /// <summary>
        /// Returns whether this circle can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetEditable()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "getEditable");
        }

        /// <summary>
        /// Returns the map on which this circle is displayed.
        /// </summary>
        /// <returns></returns>
        public MapComponent GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Returns the radius of this circle (in meters).
        /// </summary>
        /// <returns></returns>
        public Task<double> GetRadius()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<double>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "getDraggable");
        }

        /// <summary>
        /// Returns whether this circle is visible on the map.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetVisible()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "getVisible");
        }

        /// <summary>
        /// Sets the center of this circle.
        /// </summary>
        /// <param name="center"></param>
        public Task SetCenter(LatLngLiteral center)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setCenter",
                center);
        }

        /// <summary>
        /// If set to true, the user can drag this circle over the map.
        /// </summary>
        /// <param name="draggable"></param>
        public Task SetDraggable(bool draggable)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setDraggable",
                draggable);
        }

        /// <summary>
        /// If set to true, the user can edit this circle by dragging the control points shown at the center and around the circumference of the circle.
        /// </summary>
        /// <param name="editable"></param>
        public Task SetEditable(bool editable)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setEditable",
                editable);
        }

        /// <summary>
        /// Renders the circle on the specified map. If map is set to null, the circle will be removed.
        /// </summary>
        /// <param name="map"></param>
        public Task SetMap(MapComponent map)
        {
            _map = map;

            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.setMap",
                _guid.ToString(),
                map.DivId);
        }

        public Task SetOptions(CircleOptions options)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setOptions",
                options);
        }

        /// <summary>
        /// Sets the radius of this circle (in meters).
        /// </summary>
        /// <param name="radius"></param>
        public Task SetRadius(double radius)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setRadius",
                radius);
        }

        /// <summary>
        /// Hides this circle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public Task SetVisible(bool visible)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapCircleJsFunctions.invoke",
                _guid.ToString(),
                "setVisible",
                visible);
        }
    }
}
