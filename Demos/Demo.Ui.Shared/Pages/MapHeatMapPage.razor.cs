using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Visualization;

namespace Demo.Ui.Shared.Pages;

public partial class MapHeatMapPage
{
    private GoogleMap map1;

    private MapOptions mapOptions;

    protected override void OnInitialized()
    {
        mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral(13.505892, 100.8162),
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

        var hwp = new List<WeightedLocation>();
        hwp.Add(new WeightedLocation { Location = new LatLngLiteral(13.505892, 100.8142), Weight = 3 });
        hwp.Add(new WeightedLocation { Location = new LatLngLiteral(13.506892, 100.8132), Weight = 5 });

        await heatMap.SetData(hwp);

        //await heatMap.SetData(heatPoints);
    }
}