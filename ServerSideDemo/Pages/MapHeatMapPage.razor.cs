using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Visualization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

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

        var hwp = new List<WeightedLocation>();
        hwp.Add(new WeightedLocation { Location = new LatLngLiteral { Lat = 13.505892, Lng = 100.8142 }, Weight = 3 });
        hwp.Add(new WeightedLocation { Location = new LatLngLiteral { Lat = 13.506892, Lng = 100.8132 }, Weight = 5 });

        await heatMap.SetData(hwp);

        //await heatMap.SetData(heatPoints);
    }
}