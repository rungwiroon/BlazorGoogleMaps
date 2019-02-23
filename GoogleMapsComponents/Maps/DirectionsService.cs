using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A service for computing directions between two or more places.
    /// </summary>
    public class DirectionsService : JsObjectRef
    {
        private readonly string jsObjectName = "googleMapDirectionServiceFunctions";

        /// <summary>
        /// Creates a new instance of a DirectionsService that sends directions queries to Google servers.
        /// </summary>
        public DirectionsService()
        {
            JSRuntime.Current.InvokeAsync<object>(
                $"{jsObjectName}.init",
                _guid.ToString());
        }

        public override void Dispose()
        {
            JSRuntime.Current.InvokeAsync<bool>(
                    $"{jsObjectName}.dispose",
                    _guid.ToString());
        }

        /// <summary>
        /// Issue a directions search request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public async Task<DirectionResponse> Route(DirectionsRequest request)
        {
            var json = await Helper.InvokeWithDefinedGuidAsync<string>(
                    $"{jsObjectName}.route",
                    _guid.ToString(),
                    request);

            var directionResponse = JsonConvert.DeserializeObject<DirectionResponse>(json);

            return directionResponse;
        }
    }
}
