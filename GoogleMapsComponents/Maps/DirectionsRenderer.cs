using Microsoft.JSInterop;
using System.Threading.Tasks;
using static GoogleMapsComponents.Helper;

namespace GoogleMapsComponents.Maps
{
    public class DirectionsRenderer : MVCObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
        /// <returns></returns>
        public static async Task<DirectionsRenderer> CreateAsync(IJSRuntime jsRuntime, DirectionsRendererOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.DirectionsRenderer",
                opts);

            var obj = new DirectionsRenderer(jsObjectRef);
            
            return obj;
        }

        private DirectionsRenderer(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        public ValueTask<Map?> GetMap()
        {
            return this.InvokeWithReturnedObjectRefAsync(
                "getMap",
                objRef => new Map(objRef));
        }

        public ValueTask<int> GetRouteIndex()
        {
            return InvokeAsync<int>(
                "getRouteIndex");
        }

        public ValueTask SetDirections(IJSObjectReference? directions)
        {
            return this.InvokeVoidAsync(
                "setDirections",
                directions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
        /// <returns></returns>
        public ValueTask<DirectionsResult> GetDirections()
        {
            return InvokeAsync<DirectionsResult>(
                "getDirections");
        }

        public ValueTask SetMap(Map? map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                MakeArgJsFriendly(map));
        }

        public ValueTask SetRouteIndex(int routeIndex)
        {
            return this.InvokeVoidAsync(
                "setRouteIndex",
                routeIndex);
        }
    }
}
