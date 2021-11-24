using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class MultiLineString : Geometry
    {
        internal MultiLineString(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {           
        }
    }
}
