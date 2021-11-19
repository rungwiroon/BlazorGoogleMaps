using Microsoft.JSInterop;
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
        public ValueTask<ReferenceAndValue<DirectionsResult>> Route(DirectionsRequest request)
        {
            return this.InvokeAsyncReturnedReferenceAndValue<DirectionsResult>(
                "route",
                request);
        }

        public ValueTask<T> Route<T>(DirectionsRequest request)
        {
            if (typeof(T) != typeof(DirectionsResult)
                && typeof(T) != typeof(IJSObjectReference))
                throw new InvalidCastException("label type must be string or MarkerLabel.");

            return InvokeAsync<T>(
                "route",
                request);
        }
    }
}
