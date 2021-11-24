using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A Polygon geometry contains a number of Data.LinearRings. 
    /// The first linear-ring must be the polygon exterior boundary and subsequent linear-rings must be interior boundaries, also known as holes. 
    /// See the sample polygon with a hole.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class Polygon : Geometry
    {
        /// <summary>
        /// Constructs a Data.Polygon from the given Data.LinearRings or arrays of positions.
        /// </summary>
        public static async ValueTask<Polygon> CreateAsync(IJSRuntime jsRuntime, IEnumerable<IEnumerable<LatLngLiteral>> elements)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.Data.Polygon",
                elements);

            var obj = new Polygon(jsObjectRef);

            return obj;
        }

        /// <summary>
        /// Constructs a Data.Polygon from the given Data.LinearRings or arrays of positions.
        /// </summary>
        public static async ValueTask<Polygon> CreateAsync(IJSRuntime jsRuntime, IEnumerable<LinearRing> elements)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.Data.Polygon",
                elements);

            var obj = new Polygon(jsObjectRef);

            return obj;
        }

        internal Polygon(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }
    }
}
