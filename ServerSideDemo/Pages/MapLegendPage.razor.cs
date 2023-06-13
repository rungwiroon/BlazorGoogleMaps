using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ServerSideDemo.Pages;

public partial class MapLegendPage
{
    private GoogleMap map1;

    private MapOptions mapOptions;

    [Inject] private IJSRuntime jsRuntime { get; set; }

    protected ElementReference legendReference { get; set; }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await jsRuntime.InvokeAsync<object>("initMapLegend");
        }
    }

    private async Task AfterMapInit()
    {
        await map1.InteropObject.AddControl(ControlPosition.TopLeft, legendReference);
    }
}