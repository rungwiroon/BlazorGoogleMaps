using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps
{
    public class GroundOverlay : IDisposable, IJsObjectRef
    {
        private readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public Guid Guid => _jsObjectRef.Guid;

        public async static Task<GroundOverlay> CreateAsync(IJSRuntime jsRuntime, GroundOverlayOptions opts)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.GroundOverlay", opts.Url, opts.Bounds);
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
