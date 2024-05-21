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
    private ControlPosition _controlPosition = ControlPosition.TopLeft;
    [Inject] private IJSRuntime JsRuntime { get; set; }

    protected ElementReference LegendReference { get; set; }
    protected ElementReference LegendReference2 { get; set; }

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions
        {
            Zoom = 13,
            ZoomControlOptions = new ZoomControlOptions
            {
                Position = ControlPosition.BottomCenter
            },
            Center = new LatLngLiteral
            {
                Lat = 13.505892,
                Lng = 100.8162
            },
            MapTypeId = MapTypeId.Terrain,
            MapTypeControlOptions = new MapTypeControlOptions
            {
                Position = ControlPosition.TopLeft,
                Style = MapTypeControlStyle.DropdownMenu,
                MapTypeIds = new[] { MapTypeId.Roadmap, MapTypeId.Terrain, MapTypeId.Satellite, MapTypeId.Hybrid }
            }
        };
    }

    private async Task AfterMapInit()
    {
        IJSObjectReference serverSideScripts = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/serverSideScripts.js");
        await serverSideScripts.InvokeVoidAsync("initMapLegend");
    }

    private async Task AddLegend()
    {
        await _map1.InteropObject.AddControl(_controlPosition, LegendReference);
    }

    private async Task AddLegend2()
    {
        await _map1.InteropObject.AddControl(_controlPosition, LegendReference2);
    }

    private async Task RemoveLegend()
    {
        await _map1.InteropObject.RemoveControl(_controlPosition, LegendReference);
    }

    private async Task RemoveLegend2()
    {
        await _map1.InteropObject.RemoveControl(_controlPosition, LegendReference2);
    }

    private async Task RemoveAllControls()
    {
        await _map1.InteropObject.RemoveControls(_controlPosition);
    }

    private async Task HandleClick()
    {
        await JsRuntime.InvokeVoidAsync("alert", "Hello from Blazor");
    }
}