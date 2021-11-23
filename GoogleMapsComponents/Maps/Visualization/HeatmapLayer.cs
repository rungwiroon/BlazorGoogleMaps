using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Visualization
{
    /// <summary>
    /// A layer that provides a client-side rendered heatmap, depicting the intensity of data at geographical points.
    /// </summary>
    public class HeatmapLayer : Object
    {
        //private readonly string jsObjectName = "googleMapHeatmapLayerJsFunctions";

        /// <summary>
        /// Creates a new instance of HeatmapLayer.
        /// </summary>
        /// <param name="opts"></param>
        public async static Task<HeatmapLayer> CreateAsync(IJSRuntime jsRuntime, HeatmapLayerOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.visualization.HeatmapLayer", opts);

            //var obj = new HeatmapLayer(jsObjectRef, opts);

            //return obj;

            throw new NotImplementedException();
        }

        internal HeatmapLayer(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns the data points currently displayed by this heatmap.
        /// </summary>
        /// <returns></returns>
        public ValueTask<IEnumerable<object>> GetData()
        {
            return refWrapper.InvokeAsync<IEnumerable<object>>(
                "getData");
        }

        public Map GetMap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the data points to be displayed by this heatmap.
        /// </summary>
        /// <param name="data"></param>
        public ValueTask SetData(IEnumerable<LatLngLiteral> data)
        {
            return this.InvokeVoidAsync(
                "setData",
                data);
        }

        /// <summary>
        /// Sets the data points to be displayed by this heatmap.
        /// </summary>
        /// <param name="data"></param>
        public ValueTask SetData(IEnumerable<WeightedLocation> data)
        {
            return this.InvokeVoidAsync(
                "setData",
                data);
        }

        /// <summary>
        /// Renders the heatmap on the specified map. If map is set to null, the heatmap will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                map.Reference);
        }

        public ValueTask SetOptions(HeatmapLayerOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }
    }
}
