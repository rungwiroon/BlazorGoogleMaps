using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// Allows users to draw markers, polygons, polylines, rectangles, and circles on the map. 
    /// The DrawingManager's drawing mode defines the type of overlay that will be created by the user. 
    /// Adds a control to the map, allowing the user to switch drawing mode.
    /// </summary>
    public class DrawingManager : JsObjectRef
    {
        private MapComponent _map;

        /// <summary>
        /// Creates a DrawingManager that allows users to draw overlays on the map, and switch between the type of overlay to be drawn with a drawing control.
        /// </summary>
        public DrawingManager(IJSRuntime jsRuntime, DrawingManagerOptions opt = null)
            : base(jsRuntime, "google.maps.drawing.DrawingManager", opt)
        {
            if (opt?.Map != null)
                _map = opt.Map;

            _jsRuntime.MyInvokeAsync<bool>(
                "googleMapDrawingManagerJsFunctions.init",
                _guid,
                opt);
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeAsync<bool>(
                "googleMapDrawingManagerJsFunctions.dispose",
                _guid);
        }

        /// <summary>
        /// Returns the DrawingManager's drawing mode.
        /// </summary>
        /// <returns></returns>
        public async Task<OverlayType> GetDrawingMode()
        {
            var result = await _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapDrawingManagerJsFunctions.invoke",
                _guid.ToString(),
                "getDrawingMode");

            return Helper.ToEnum<OverlayType>(result);
        }

        /// <summary>
        /// Returns the Map to which the DrawingManager is attached, which is the Map on which the overlays created will be placed.
        /// </summary>
        /// <returns></returns>
        public MapComponent GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Changes the DrawingManager's drawing mode, which defines the type of overlay to be added on the map. 
        /// Accepted values are 'marker', 'polygon', 'polyline', 'rectangle', 'circle', or null. 
        /// A drawing mode of null means that the user can interact with the map as normal, and clicks do not draw anything.
        /// </summary>
        /// <returns></returns>
        public Task SetDrawingMode(OverlayType drawingMode)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<object>(
                   "googleMapDrawingManagerJsFunctions.invoke",
                   _guid.ToString(),
                   "setDrawingMode",
                   drawingMode);
        }

        /// <summary>
        /// Attaches the DrawingManager object to the specified Map.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMap(MapComponent map)
        {
            await _jsRuntime.MyInvokeAsync<bool>(
                   "googleMapDrawingManagerJsFunctions.setMap",
                   _guid,
                   map?.DivId);

            _map = map;
        }

        /// <summary>
        /// Sets the DrawingManager's options.
        /// </summary>
        /// <param name="options"></param>
        public Task SetOptions(DrawingManagerOptions options)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<object>(
                   "googleMapDrawingManagerJsFunctions.invoke",
                   _guid.ToString(),
                   "setOptions",
                   options);
        }
    }
}
