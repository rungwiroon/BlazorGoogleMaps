using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A LatLngBounds instance represents a rectangle in geographical coordinates,
    /// including one that crosses the 180 degrees longitudinal meridian.
    /// </summary>
    public class LatLngBounds : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        /// <summary>
        /// Constructs a rectangle from the points at its south-west and north-east corners..
        /// </summary>
        /// <param name="sw">South west corner</param>
        /// <param name="ne">North east corner</param>
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

        public void Dispose()
        {
            _jsObjectRef.Dispose();
        }

        /// <summary>
        /// Extends this bounds to contain the given point.
        /// </summary>
        /// <returns></returns>
        public Task Extend(LatLngLiteral point)
        {
            return _jsObjectRef.InvokeAsync("extend", point);
        }
        
        /// <summary>
        /// Computes the center of this LatLngBounds
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetCenter()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getCenter");
        }

        /// <summary>
        /// Returns the north-east corner of this bounds.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetNorthEast()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getNorthEast");
        }

        /// <summary>
        /// Returns the south-west corner of this bounds.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetSouthWest()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getSouthWest");
        }
        
        /// <summary>
        /// Returns the literal representation of this bounds
        /// </summary>
        /// <returns></returns>
        public Task<LatLngBoundsLiteral> ToJson()
        {
            return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("toJSON");
        }

        /// <summary>
        /// Returns true if this bounds shares any points with the other bounds.
        /// </summary>
        /// <returns></returns>
        public Task<bool> Intersects(LatLngBoundsLiteral other)
        {
            return _jsObjectRef.InvokeAsync<bool>("intersects", other);
        }
        
        /// <summary>
        /// Returns true if the bounds are empty.
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsEmpty()
        {
            return _jsObjectRef.InvokeAsync<bool>("isEmpty");
        }
    }
}
