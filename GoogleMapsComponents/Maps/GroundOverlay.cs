using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps
{
    public class GroundOverlay : IDisposable, IJsObjectRef
    {
        private readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public Guid Guid => _jsObjectRef.Guid;

        public async static Task<GroundOverlay> CreateAsync(IJSRuntime jsRuntime, string url, LatLngBoundsLiteral bounds, GroundOverlayOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.GroundOverlay", url, bounds, opts);
            var obj = new GroundOverlay(jsObjectRef);

            return obj;
        }

        internal GroundOverlay(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
            EventListeners = new Dictionary<string, List<MapEventListener>>();
        }

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            JsObjectRef listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }
            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        /// <summary>
        /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
        /// </summary>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public async Task SetOpacity(double opacity)
        {
            if (opacity > 1) return;
            if (opacity < 0) return;

            await _jsObjectRef.InvokeAsync("setOpacity", opacity);
        }

        /// <summary>
        /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
        /// </summary>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public async Task SetOpacity(decimal opacity)
        {
            await SetOpacity(Convert.ToDouble(opacity));
        }

        public async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync(
                "setMap",
                map);

            //_map = map;
        }

        public void Dispose()
        {
            _jsObjectRef?.Dispose();
        }
    }
}
