using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A LatLngBounds instance represents a rectangle in geographical coordinates,
    /// including one that crosses the 180 degrees longitudinal meridian.
    /// </summary>
    public class LatLngBounds : IAsyncDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        /// <summary>
        /// Constructs a new empty bounds
        /// </summary>
        public async static Task<LatLngBounds> CreateAsync(IJSRuntime jsRuntime)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.LatLngBounds");

            var obj = new LatLngBounds(jsObjectRef);

            return obj;
        }

        private LatLngBounds(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public ValueTask DisposeAsync()
        {
            return _jsObjectRef.DisposeAsync();
        }

        /// <summary>
        /// Returns true if the given lat/lng is in this bounds.
        /// </summary>
        public ValueTask<bool> Contains(LatLngLiteral other)
        {
            return _jsObjectRef.InvokeAsync<bool>("contains", other);
        }

        /// <summary>
        /// Returns true if this bounds approximately equals the given bounds.
        /// </summary>
        public ValueTask<bool> Equals(LatLngBoundsLiteral other)
        {
            return _jsObjectRef.InvokeAsync<bool>("equals", other);
        }

        /// <summary>
        /// Extends this bounds to contain the given point.
        /// </summary>
        public ValueTask Extend(LatLngLiteral point)
        {
            return _jsObjectRef.InvokeAsync("extend", point);
        }
        
        /// <summary>
        /// Computes the center of this LatLngBounds
        /// </summary>
        public ValueTask<LatLngLiteral> GetCenter()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getCenter");
        }

        /// <summary>
        /// Returns the north-east corner of this bounds.
        /// </summary>
        public ValueTask<LatLngLiteral> GetNorthEast()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getNorthEast");
        }

        /// <summary>
        /// Returns the south-west corner of this bounds.
        /// </summary>
        public ValueTask<LatLngLiteral> GetSouthWest()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getSouthWest");
        }
        
        /// <summary>
        /// Returns true if this bounds shares any points with the other bounds.
        /// </summary>
        public ValueTask<bool> Intersects(LatLngBoundsLiteral other)
        {
            return _jsObjectRef.InvokeAsync<bool>("intersects", other);
        }

        /// <summary>
        /// Returns true if the bounds are empty.
        /// </summary>
        public ValueTask<bool> IsEmpty()
        {
            return _jsObjectRef.InvokeAsync<bool>("isEmpty");
        }

        /// <summary>
        /// Returns the literal representation of this bounds
        /// </summary>
        public ValueTask<LatLngBoundsLiteral> ToJson()
        {
            return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("toJSON");
        }

        /// <summary>
        /// Converts the given map bounds to a lat/lng span.
        /// </summary>
        public ValueTask<LatLngLiteral> ToSpan()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("toSpan");
        }

        /// <summary>
        /// Returns a string of the form "lat_lo,lng_lo,lat_hi,lng_hi" for this bounds,
        /// where "lo" corresponds to the southwest corner of the bounding box, while "hi"
        /// corresponds to the northeast corner of that box.
        /// </summary>
        public ValueTask<string> ToUrlValue(double precision)
        {
            return _jsObjectRef.InvokeAsync<string>("toUrlValue", precision);
        }

        /// <summary>
        /// Returns a string of the form "lat_lo,lng_lo,lat_hi,lng_hi" for this bounds,
        /// where "lo" corresponds to the southwest corner of the bounding box, while "hi"
        /// corresponds to the northeast corner of that box.
        /// </summary>
        public ValueTask<string> ToUrlValue(decimal precision)
        {
            return ToUrlValue(Convert.ToDouble(precision));
        }

        /// <summary>
        /// Extends this bounds to contain the union of this and the given bounds.
        /// </summary>
        public ValueTask Union(LatLngBoundsLiteral other)
        {
            return _jsObjectRef.InvokeAsync("union", other);
        }
    }
}
