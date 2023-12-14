using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapLegendPage
{
    private GoogleMap map1;

    private MapOptions mapOptions;

    private ControlPosition controlPosition = ControlPosition.TopLeft;

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
    }

    private async Task RemoveLegend()
    {
        await map1.InteropObject.RemoveControl(controlPosition, legendReference);
    }

    private async Task RemoveAllControls()
    {
        await map1.InteropObject.RemoveControls(controlPosition);
    }

    private async Task AddLegend()
    {
        await map1.InteropObject.AddControl(controlPosition, legendReference);
    }
}