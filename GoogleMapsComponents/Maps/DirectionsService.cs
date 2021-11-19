using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A service for computing directions between two or more places.
    /// </summary>
    public class DirectionsService : JsObjectRef
    {
        /// <summary>
        /// Creates a new instance of a DirectionsService that sends directions queries to Google servers.
        /// </summary>
        public async static Task<DirectionsService> CreateAsync(IJSRuntime jsRuntime)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.DirectionsService");

            var obj = new DirectionsService(jsObjectRef);

            return obj;
        }

        internal DirectionsService(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Issue a directions search request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
        /// <returns></returns>
        public async Task<DirectionsResult> Route(DirectionsRequest request, DirectionsRequestOptions? directionsRequestOptions = null)
        {
            var response = await InvokeAsync<string>(
                "googleMapDirectionServiceFunctions.route",
                request,
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
    }
}
