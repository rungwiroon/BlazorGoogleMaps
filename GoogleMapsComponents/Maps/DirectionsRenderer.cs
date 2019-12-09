using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps
{
    public class DirectionsRenderer : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        public static async Task<DirectionsRenderer> CreateAsync(IJSRuntime jsRuntime, DirectionsRendererOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.DirectionsRenderer", opts);
            var obj = new DirectionsRenderer(jsObjectRef);

            return obj;
        }

        private DirectionsRenderer(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public void Dispose()
        {
            _jsObjectRef?.Dispose();
        }

        public async Task Route(DirectionsRequest request)
        {
            await _jsObjectRef.InvokeAsync(
                    "googleMapDirectionServiceFunctions.route",
                    request);
        }

        public async Task<DirectionsResult> GetAnimation()
        {
            return await _jsObjectRef.InvokeAsync<DirectionsResult>(
                "getDirections");

        }

        public Task<Map> GetMap()
        {
            return _jsObjectRef.InvokeAsync<Map>(
                "getMap");
        }

        public Task<int> GetRouteIndex()
        {
            return _jsObjectRef.InvokeAsync<int>(
                "getRouteIndex");
        }

        public async Task SetDirections(DirectionsResult directions)
        {
            await _jsObjectRef.InvokeAsync(
                "setDirections",
                directions);
        }

        public async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync(
                   "setMap",
                   map);
        }

        public async Task SetRouteIndex(int routeIndex)
        {
            await _jsObjectRef.InvokeAsync(
                "setRouteIndex",
                routeIndex);
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
