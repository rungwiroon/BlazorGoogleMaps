using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
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
                "googleMapsObjectManager.createObject",
                "google.maps.DirectionsRenderer",
                opts);
            var obj = new DirectionsRenderer(jsObjectRef);
            return obj;
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
        //public async Task<DirectionsResult> Route(DirectionsRequest request, DirectionsRequestOptions directionsRequestOptions = null)
        //{
        //    if (directionsRequestOptions == null)
        //    {
        //        directionsRequestOptions = new DirectionsRequestOptions();
        //    }

        //    var response = await InvokeAsync<string>(
        //        "googleMapDirectionServiceFunctions.route",
        //        request,
        //        directionsRequestOptions);
        //    try
        //    {
        //        var dirResult = JsonConvert.DeserializeObject<DirectionsResult>(response);
        //        return dirResult;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error parsing DirectionsResult Object. Message: " + e.Message);
        //        return null;
        //    }
        //}

        public ValueTask<Map?> GetMap()
        {
            return InvokeWithReturnedObjectRefAsync(
                "getMap",
                objRef => new Map(objRef));
        }

        public ValueTask<int> GetRouteIndex()
        {
            return InvokeAsync<int>(
                "getRouteIndex");
        }

        public async ValueTask SetDirections(IJSObjectReference? directions)
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
        public async ValueTask<DirectionsResult> GetDirections()
        {
            return await InvokeAsync<DirectionsResult>(
                "getDirections");
        }

        public async ValueTask SetMap(Map? map)
        {
            await InvokeVoidAsync(
                "setMap",
                MakeArgJsFriendly(map));
        }

        public async ValueTask SetRouteIndex(int routeIndex)
        {
            await InvokeVoidAsync(
                "setRouteIndex",
                routeIndex);
        }
    }
}
