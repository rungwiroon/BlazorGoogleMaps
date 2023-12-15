using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapLegendPage
{
    private GoogleMap _map1;

    private MapOptions _mapOptions;

    [Inject] private IJSRuntime JsRuntime { get; set; }

    protected ElementReference LegendReference { get; set; }

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
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
            await JsRuntime.InvokeAsync<object>("initMapLegend");
        }
    }

    private Task AfterMapInit()
    {
        return Task.CompletedTask;
    }

    private async Task RemoveLegend()
    {
        await _map1.InteropObject.RemoveControl(ControlPosition.TopLeft, LegendReference);
    }

    private async Task RemoveAllControls()
    {
        await _map1.InteropObject.RemoveControls(ControlPosition.TopLeft);
    }

    private async Task AddLegend()
    {
        await _map1.InteropObject.AddControl(ControlPosition.TopLeft, LegendReference);
    }
}