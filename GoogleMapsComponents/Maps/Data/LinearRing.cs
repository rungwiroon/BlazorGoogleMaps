using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A LinearRing geometry contains a number of LatLngs, representing a closed LineString. 
    /// There is no need to make the first LatLng equal to the last LatLng. The LinearRing is closed implicitly.
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class LinearRing : Geometry
    {
        public static async ValueTask<LinearRing> CreateAsync(
            IJSRuntime jsRuntime, IEnumerable<LatLngLiteral> elements)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.Data.LinearRing",
                elements);

            var obj = new LinearRing(jsObjectRef);

            return obj;
        }

        internal LinearRing(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }
    }
}
