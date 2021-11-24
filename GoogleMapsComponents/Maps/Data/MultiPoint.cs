using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A MultiLineString geometry contains a number of LineString s.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class MultiPoint : Geometry
    {
        internal MultiPoint(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {           
        }

        /// <summary>
        /// Returns an array of the contained LatLngs. 
        /// A new array is returned each time getArray() is called.
        /// </summary>
        /// <returns></returns>
        public List<LatLngLiteral> GetArray()
        {
            throw new NotImplementedException();
        }
    }
}
