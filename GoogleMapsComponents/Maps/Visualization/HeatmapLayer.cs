using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Visualization
{
    /// <summary>
    /// A layer that provides a client-side rendered heatmap, depicting the intensity of data at geographical points.
    /// </summary>
    public class HeatmapLayer : JsObjectRef
    {
        private MapComponent _map;

        private string jsObjectName = "googleMapHeatmapLayerJsFunctions";

        /// <summary>
        /// Creates a new instance of HeatmapLayer.
        /// </summary>
        /// <param name="opts"></param>
        public HeatmapLayer(HeatmapLayerOptions opts = null)
        {
            if (opts != null)
            {
                _map = opts.Map;

                Helper.InvokeWithDefinedGuidAsync<bool>(
                    $"{jsObjectName}.init",
                    _guid.ToString(),
                    opts);
            }
            else
            {
                Helper.InvokeWithDefinedGuidAsync<bool>(
                    $"{jsObjectName}.init",
                    _guid.ToString());
            }
        }

        public override void Dispose()
        {
            Helper.InvokeWithDefinedGuidAsync<bool>(
                    $"{jsObjectName}.dispose",
                    _guid.ToString());
        }

        /// <summary>
        /// Returns the data points currently displayed by this heatmap.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<object>> GetData()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<IEnumerable<object>>(
                $"{jsObjectName}.invoke",
                _guid.ToString(),
                "getData");
        }

        public MapComponent GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Sets the data points to be displayed by this heatmap.
        /// </summary>
        /// <param name="data"></param>
        public Task SetData(IEnumerable<LatLngLiteral> data)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                $"{jsObjectName}.invoke",
                _guid.ToString(),
                "setData",
                data);
        }

        /// <summary>
        /// Sets the data points to be displayed by this heatmap.
        /// </summary>
        /// <param name="data"></param>
        public Task SetData(IEnumerable<WeightedLocation> data)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                $"{jsObjectName}.invoke",
                _guid.ToString(),
                "setData",
                data);
        }

        /// <summary>
        /// Renders the heatmap on the specified map. If map is set to null, the heatmap will be removed.
        /// </summary>
        /// <param name="map"></param>
        public Task SetMap(MapComponent map)
        {
            _map = map;

            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                $"{jsObjectName}.setMap",
                _guid.ToString(),
                map.DivId);
        }

        public Task SetOptions(HeatmapLayerOptions options)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                $"{jsObjectName}.invoke",
                _guid.ToString(),
                "setOptions",
                options);
        }
    }
}
