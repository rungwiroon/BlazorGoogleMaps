using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A GeometryCollection contains a number of geometry objects. 
    /// Any LatLng or LatLngLiteral objects are automatically converted to Data.Point geometry objects.
    /// </summary>
    public class GeometryCollection : Geometry
    {
        internal GeometryCollection(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }
    }
}
