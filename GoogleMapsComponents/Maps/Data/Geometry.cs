using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A superclass for the various geometry objects.
    /// </summary>
    public abstract class Geometry : Object
    {
        internal Geometry(IJSObjectReference jsObjectRef) : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Repeatedly invokes the given function, passing a point from the geometry to the function on each invocation.
        /// </summary>
        /// <returns></returns>
        public void ForEachLatLng()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the type of the geometry object.
        /// Possibilities are "Point", "MultiPoint", "LineString", "MultiLineString", "LinearRing", "Polygon", "MultiPolygon", or "GeometryCollection"
        /// </summary>
        public ValueTask<string> GetGeometryType()
        {
            return this.InvokeAsync<string>("getType");
        }
    }
}
