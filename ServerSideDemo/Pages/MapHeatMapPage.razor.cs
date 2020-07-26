using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Visualization;
using ServerSideDemo.Shared;

namespace ServerSideDemo.Pages
{
    public partial class MapHeatMapPage
    {
        private GoogleMap map1;

        private MapOptions mapOptions;

        protected override void OnInitialized()
        {
            mapOptions = new MapOptions()
            {
                Zoom = 13,
                Center = new LatLngLiteral()
                {
                    Lat = 13.505892,
                    Lng = 100.8162
                },
                MapTypeId = MapTypeId.Roadmap
            };
        }

        private async Task AddHeatMap()
        {


            var heatMap = await HeatmapLayer.CreateAsync(map1.JsRuntime, new HeatmapLayerOptions
            {
                Map = map1.InteropObject,
                Dissipating = true,
                Radius = 10,
            });

            var heatPoints = new[] {
                new LatLngLiteral{
                    Lat = 13.505892,
                    Lng = 100.8162,
                },
                new LatLngLiteral{
                    Lat = 13.506892,
                    Lng = 100.8162,
                },
            };

            await heatMap.SetData(heatPoints);
        }
    }
}
