using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A LatLngBounds instance represents a rectangle in geographical coordinates,
    /// including one that crosses the 180 degrees longitudinal meridian.
    /// </summary>
    public class LatLngBounds : Object
    {
        /// <summary>
        /// Constructs a new empty bounds
        /// </summary>
        public async static ValueTask<LatLngBounds> CreateAsync(
            IJSRuntime jsRuntime, LatLngLiteral? sw, LatLngLiteral? ne)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.LatLngBounds",
                new LatLngLiteral?[] { sw, ne });

            var obj = new LatLngBounds(jsObjectRef);

            return obj;
        }

        internal LatLngBounds(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns true if the given lat/lng is in this bounds.
        /// </summary>
        public ValueTask<bool> Contains(LatLngLiteral other)
        {
            return this.InvokeAsync<bool>("contains", other);
        }

        /// <summary>
        /// Returns true if this bounds approximately equals the given bounds.
        /// </summary>
        public ValueTask<bool> Equals(LatLngBoundsLiteral other)
        {
            return this.InvokeAsync<bool>("equals", other);
        }

        /// <summary>
        /// Extends this bounds to contain the given point.
        /// </summary>
        public ValueTask Extend(LatLngLiteral point)
        {
            return this.InvokeVoidAsync("extend", point);
        }
        
        /// <summary>
        /// Computes the center of this LatLngBounds
        /// </summary>
        public ValueTask<LatLngLiteral> GetCenter()
        {
            return this.InvokeAsync<LatLngLiteral>("getCenter");
        }

        /// <summary>
        /// Returns the north-east corner of this bounds.
        /// </summary>
        public ValueTask<LatLngLiteral> GetNorthEast()
        {
            return this.InvokeAsync<LatLngLiteral>("getNorthEast");
        }

        /// <summary>
        /// Returns the south-west corner of this bounds.
        /// </summary>
        public ValueTask<LatLngLiteral> GetSouthWest()
        {
            return this.InvokeAsync<LatLngLiteral>("getSouthWest");
        }
        
        /// <summary>
        /// Returns true if this bounds shares any points with the other bounds.
        /// </summary>
        public ValueTask<bool> Intersects(LatLngBoundsLiteral other)
        {
            return this.InvokeAsync<bool>("intersects", other);
        }

        /// <summary>
        /// Returns true if the bounds are empty.
        /// </summary>
        public ValueTask<bool> IsEmpty()
        {
            return this.InvokeAsync<bool>("isEmpty");
        }

        /// <summary>
        /// Returns the literal representation of this bounds
        /// </summary>
        public ValueTask<LatLngBoundsLiteral> ToJson()
        {
            return this.InvokeAsync<LatLngBoundsLiteral>("toJSON");
        }

        /// <summary>
        /// Converts the given map bounds to a lat/lng span.
        /// </summary>
        public ValueTask<LatLngLiteral> ToSpan()
        {
            return this.InvokeAsync<LatLngLiteral>("toSpan");
        }

        /// <summary>
        /// Returns a string of the form "lat_lo,lng_lo,lat_hi,lng_hi" for this bounds,
        /// where "lo" corresponds to the southwest corner of the bounding box, while "hi"
        /// corresponds to the northeast corner of that box.
        /// </summary>
        public ValueTask<string> ToUrlValue(double precision)
        {
            return this.InvokeAsync<string>("toUrlValue", precision);
        }

        /// <summary>
        /// Extends this bounds to contain the union of this and the given bounds.
        /// </summary>
        public ValueTask Union(LatLngBoundsLiteral other)
        {
            return this.InvokeVoidAsync("union", other);
        }
    }
}
