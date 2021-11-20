using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    [Flags]
    public enum StripOption
    {
        OverviewPath = 0x01,
        OverviewPolyline = 0x02,
        LegsSteps = 0x04,
        LegsStepsLatLngs = 0x08,
        LegsStepsPath = 0x10,
    }

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
        public ValueTask<ReferenceAndValue<DirectionsResult>> Route(
            DirectionsRequest request,
            StripOption? stripOption = 
                StripOption.OverviewPath
                | StripOption.OverviewPolyline
                | StripOption.LegsStepsLatLngs
                | StripOption.LegsStepsPath)
        {
            return this.InvokeAsyncReturnedReferenceAndValue<DirectionsResult>(
                "route",
                mapStripOptionToPropertyPath(stripOption).ToArray(),
                request);

            static IEnumerable<string> mapStripOptionToPropertyPath(StripOption? stripOption)
            {
                if (stripOption == null)
                    yield break;

                if((stripOption & StripOption.OverviewPath) > 0)
                    yield return "[routes].overview_path";

                if((stripOption & StripOption.OverviewPolyline) > 0)
                    yield return "[routes].overview_polyline";

                if ((stripOption & StripOption.LegsSteps) > 0)
                    yield return "[routes].[legs].steps";

                if ((stripOption & StripOption.LegsStepsLatLngs) > 0)
                    yield return "[routes].[legs].[steps].lat_lngs";

                if ((stripOption & StripOption.LegsStepsPath) > 0)
                    yield return "[routes].[legs].[steps].path";
            }
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
