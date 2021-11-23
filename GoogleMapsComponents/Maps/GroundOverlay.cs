using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class GroundOverlay : MVCObject
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

        /// <summary>
        /// The opacity of the overlay, expressed as a number between 0 and 1. Optional. Defaults to 1.
        /// </summary>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public async Task SetOpacity(double opacity)
        {
            if (opacity > 1) return;
            if (opacity < 0) return;

            await this.InvokeVoidAsync("setOpacity", opacity);
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
            await this.InvokeVoidAsync(
                "setMap",
                map.Reference);
        }
    }
}
