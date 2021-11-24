using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A MultiPolygon geometry contains a number of Data.Polygon s.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class MultiPolygon : Geometry
    {
        internal MultiPolygon(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {           
        }
    }
}
