using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class DirectionsRenderer : JsObjectRef
    {
        public static async Task<DirectionsRenderer> CreateAsync(IJSRuntime jsRuntime, DirectionsRendererOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.DirectionsRenderer", opts);
            //var obj = new DirectionsRenderer(jsObjectRef);

            //return obj;

            throw new NotImplementedException();
        }

        private DirectionsRenderer(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
        /// <returns></returns>
        public async Task<DirectionsResult> Route(DirectionsRequest request, DirectionsRequestOptions directionsRequestOptions = null)
        {
            if (directionsRequestOptions == null)
            {
                directionsRequestOptions = new DirectionsRequestOptions();
            }

            var response = await InvokeAsync<string>(
                "googleMapDirectionServiceFunctions.route",
                request, directionsRequestOptions);
            try
            {
                var dirResult = JsonConvert.DeserializeObject<DirectionsResult>(response);
                return dirResult;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing DirectionsResult Object. Message: " + e.Message);
                return null;
            }
        }

        public ValueTask<Map> GetMap()
        {
            return InvokeAsync<Map>(
                "getMap");
        }

        public ValueTask<int> GetRouteIndex()
        {
            return InvokeAsync<int>(
                "getRouteIndex");
        }

        public async ValueTask SetDirections(DirectionsResult directions)
        {
            await InvokeVoidAsync(
                "setDirections",
                directions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
        /// <returns></returns>
        public async ValueTask<DirectionsResult> GetDirections(DirectionsRequestOptions directionsRequestOptions = null)
        {
            if (directionsRequestOptions == null)
            {
                directionsRequestOptions = new DirectionsRequestOptions();
            }

            var response = await InvokeAsync<string>(
                "getDirections",
                directionsRequestOptions);
            try
            {
                var dirResult = JsonConvert.DeserializeObject<DirectionsResult>(response);
                return dirResult;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing DirectionsResult Object. Message: " + e.Message);
                return null;
            }
        }

        public async ValueTask SetMap(Map map)
        {
            await InvokeVoidAsync(
                   "setMap",
                   map);
        }

        public async ValueTask SetRouteIndex(int routeIndex)
        {
            await InvokeVoidAsync(
                "setRouteIndex",
                routeIndex);
        }

        public async ValueTask<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async ValueTask<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

    }
}
