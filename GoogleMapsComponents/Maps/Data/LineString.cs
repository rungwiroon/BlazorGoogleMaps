using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A LinearRing geometry contains a number of LatLngs, representing a closed LineString. 
    /// There is no need to make the first LatLng equal to the last LatLng. 
    /// The LinearRing is closed implicitly.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class LineString : Geometry
    {
        internal LineString(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {           
        }

        public List<LatLngLiteral> GetArray()
        {
            throw new NotImplementedException();
        }
    }
}
