﻿using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// Allows users to draw markers, polygons, polylines, rectangles, and circles on the map. 
    /// The DrawingManager's drawing mode defines the type of overlay that will be created by the user. 
    /// Adds a control to the map, allowing the user to switch drawing mode.
    /// </summary>
    public class DrawingManager : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;
        private Map _map;

        /// <summary>
        /// Creates a DrawingManager that allows users to draw overlays on the map, and switch between the type of overlay to be drawn with a drawing control.
        /// </summary>
        public async static Task<DrawingManager> CreateAsync(IJSRuntime jsRuntime, DrawingManagerOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.drawing.DrawingManager", opts);

            var obj = new DrawingManager(jsObjectRef, opts);

            return obj;
        }

        /// <summary>
        /// Creates a DrawingManager that allows users to draw overlays on the map, and switch between the type of overlay to be drawn with a drawing control.
        /// </summary>
        private DrawingManager(JsObjectRef jsObjectRef, DrawingManagerOptions opt = null)
        {
            _jsObjectRef = jsObjectRef;

            if (opt?.Map != null)
                _map = opt.Map;
        }

        public void Dispose()
        {
            _jsObjectRef.Dispose();
        }

        /// <summary>
        /// Returns the DrawingManager's drawing mode.
        /// </summary>
        /// <returns></returns>
        public async Task<OverlayType> GetDrawingMode()
        {
            var result = await _jsObjectRef.InvokeAsync<string>("getDrawingMode");

            return Helper.ToEnum<OverlayType>(result);
        }

        /// <summary>
        /// Returns the Map to which the DrawingManager is attached, which is the Map on which the overlays created will be placed.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Returns a polygon object when a polygon is added to this drawing manager
        /// </summary>
        /// <param name="jSRuntime"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task AddPolygonCompleteListener(IJSRuntime jSRuntime,  Action<Polygon> handler)
        {
            await _jsObjectRef.InvokeAsync("addListener", "polygoncomplete"); //add the listener that removes the polygons from the map. This could also be done on create and applied to polyline, circle, and other shape types
            await AddListener<OverlayCompleteEvent>("overlaycomplete", async (arg) => //internally create an overlaycomplete listener
            {
                var result = JObject.Parse(arg.Overlay.ToString());
                var LatLngList = JsonSerializer.Deserialize<IEnumerable<LatLngLiteral>>(result["latLngs"]["je"][0]["je"].ToString().Replace("l", "L")); //parse this literal to string and pull out the lat/lng pairs
                var polygon = await Polygon.CreateAsync(jSRuntime, new PolygonOptions() //create a polygon object
                {
                    Draggable = true,
                    Editable = true,
                    Map = _map
                });
                await polygon.SetPath(LatLngList); //set the pairs on this polygon
                handler.Invoke(polygon); //alert the handler that we hace a polygon
            });
        }

        /// <summary>
        /// Changes the DrawingManager's drawing mode, which defines the type of overlay to be added on the map. 
        /// Accepted values are 'marker', 'polygon', 'polyline', 'rectangle', 'circle', or null. 
        /// A drawing mode of null means that the user can interact with the map as normal, and clicks do not draw anything.
        /// </summary>
        /// <returns></returns>
        public Task SetDrawingMode(OverlayType? drawingMode)
        {
            return _jsObjectRef.InvokeAsync(
                "setDrawingMode",
                drawingMode);
        }

        /// <summary>
        /// Attaches the DrawingManager object to the specified Map.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync(
                   "googleMapDrawingManagerJsFunctions.setMap",
                   map);

            _map = map;
        }

        /// <summary>
        /// Sets the DrawingManager's options.
        /// </summary>
        /// <param name="options"></param>
        public Task SetOptions(DrawingManagerOptions options)
        {
            return _jsObjectRef.InvokeAsync(
                   "setOptions",
                   options);
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
