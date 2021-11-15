using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class GroundOverlay : JsObjectRef
    {
        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public async static Task<GroundOverlay> CreateAsync(
            IJSRuntime jsRuntime, string url, LatLngBoundsLiteral bounds, GroundOverlayOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.GroundOverlay", url, bounds, opts);
            //var obj = new GroundOverlay(jsObjectRef);

            //return obj;

            throw new NotImplementedException();
        }

        internal GroundOverlay(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
            EventListeners = new();
        }

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>("addListener", eventName, handler);
            var eventListener = new MapEventListener(listenerRef);

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

            await InvokeVoidAsync("setOpacity", opacity);
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
            await InvokeVoidAsync(
                "setMap",
                map);
        }
    }
}
