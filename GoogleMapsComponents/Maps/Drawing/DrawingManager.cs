using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// Allows users to draw markers, polygons, polylines, rectangles, and circles on the map. 
    /// The DrawingManager's drawing mode defines the type of overlay that will be created by the user. 
    /// Adds a control to the map, allowing the user to switch drawing mode.
    /// </summary>
    public class DrawingManager : MVCObject
    {
        /// <summary>
        /// Creates a DrawingManager that allows users to draw overlays on the map, and switch between the type of overlay to be drawn with a drawing control.
        /// </summary>
        public async static Task<DrawingManager> CreateAsync(
            IJSRuntime jsRuntime, DrawingManagerOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.drawing.DrawingManager",
                opts);

            var obj = new DrawingManager(jsObjectRef);

            return obj;
        }

        internal DrawingManager(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns the DrawingManager's drawing mode.
        /// </summary>
        /// <returns></returns>
        public async Task<OverlayType> GetDrawingMode()
        {
            var result = await this.InvokeAsync<string>("getDrawingMode");
            return Helper.ToEnum<OverlayType>(result);
        }

        /// <summary>
        /// Returns the Map to which the DrawingManager is attached, which is the Map on which the overlays created will be placed.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<Map> GetMap()
        {
            var mapRef = await this.InvokeAsync<IJSObjectReference>("getMap");
            return new Map(mapRef);
        }

        /// <summary>
        /// Changes the DrawingManager's drawing mode, which defines the type of overlay to be added on the map. 
        /// Accepted values are 'marker', 'polygon', 'polyline', 'rectangle', 'circle', or null. 
        /// A drawing mode of null means that the user can interact with the map as normal, and clicks do not draw anything.
        /// </summary>
        /// <returns></returns>
        public ValueTask SetDrawingMode(OverlayType? drawingMode)
        {
            return this.InvokeVoidAsync(
                "setDrawingMode",
                drawingMode);
        }

        /// <summary>
        /// Attaches the DrawingManager object to the specified Map.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                map.Reference);
        }

        /// <summary>
        /// Sets the DrawingManager's options.
        /// </summary>
        /// <param name="options"></param>
        public ValueTask SetOptions(DrawingManagerOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }

        static T CreateGeometry<T>(
            string eventName, IJSObjectReference objRef)
        {
            object obj = eventName switch
            {
                "circlecomplete" => new Circle(objRef),
                "markercomplete" => new Marker(objRef),
                "polygoncomplete" => new Polygon(objRef),
                "polylinecomplete" => new Polyline(objRef),
                "rectanglecomplete" => new Rectangle(objRef),
                _ => throw new NotImplementedException(),
            };

            return (T)obj;
        }

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public override ValueTask<MapEventListener> AddListenerAsync<T>(
            string eventName,
            Action<T> handler)
        {
            var listener = eventName switch
            {
                "overlaycomplete" =>
                    this.AddListenerAsync(
                        eventName,
                        new string[] { "overlay" },
                        DotNetObjectReference.Create(
                            new JSInvokableAction<T>(handler))),
                _ =>
                    this.AddListenerAsync(
                        eventName,
                        DotNetObjectReference.Create(
                            new JSInvokableAction<IJSObjectReference>(WrapHandler))),
            };

            return listener;

            void WrapHandler(IJSObjectReference objRef)
            {
                var obj = CreateGeometry<T>(eventName, objRef);

                handler(obj);
            }
        }

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public override ValueTask<MapEventListener> AddListenerAsync<T>(
            string eventName,
            Func<T, Task> handler)
        {
            var listener = eventName switch
            {
                "overlaycomplete" =>
                    this.AddAsyncListenerAsync(
                        eventName,
                        new string[] { "overlay" },
                        DotNetObjectReference.Create(
                            new JSInvokableAsyncAction<T>(handler))),
                _ =>
                    this.AddAsyncListenerAsync(
                        eventName,
                        DotNetObjectReference.Create(
                            new JSInvokableAsyncAction<IJSObjectReference>(WrapHandler))),
            };

            return listener;

            async Task WrapHandler(IJSObjectReference objRef)
            {
                var obj = CreateGeometry<T>(eventName, objRef);

                await handler(obj);
            }
        }
    }
}
