using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// The assumptions to use when predicting duration in traffic. 
    /// Specified as part of a DirectionsRequest or DistanceMatrixRequest. 
    /// </summary>

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TrafficModel
    {
        /// <summary>
        /// Use historical traffic data to best estimate the time spent in traffic.
        /// </summary>
        bestguess,

        /// <summary>
        /// Use historical traffic data to make an optimistic estimate of what the duration in traffic will be.
        /// </summary>
        optimistic,

        /// <summary>
        /// Use historical traffic data to make a pessimistic estimate of what the duration in traffic will be.
        /// </summary>
        pessimistic
    }
}
