using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A Point geometry contains a single LatLng.
    /// </summary>
    public class Point : Geometry
    {
        internal Point(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {           
        }

        /// <summary>
        /// Returns the contained LatLng.
        /// </summary>
        /// <returns></returns>
        public LatLngLiteral Get()
        {
            throw new NotImplementedException();
        }
    }
}
