using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using ServerSideDemo.Shared;

namespace ServerSideDemo.Pages;

public partial class MapGroundOverlayPage
{
    private GoogleMap map1;

    private MapOptions mapOptions;

    private Stack<Marker> markers = new Stack<Marker>();

    private List<String> _events = new List<String>();

    private MapEventList eventList;

    private LatLngBounds bounds;
    private GroundOverlay _mapoverlay;

    protected override void OnInitialized()
    {
        mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral()
            {
                Lat = 40.719051,
                Lng = -74.166269
            },
            MapTypeId = MapTypeId.Roadmap
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            bounds = await LatLngBounds.CreateAsync(map1.JsRuntime);
        }
    }

    private async Task RemoveOverlay()
    {
        await _mapoverlay.SetMap(null);
    }
    private async Task AddOverlay()
    {
        _mapoverlay = await GroundOverlay.CreateAsync(map1.JsRuntime, "https://www.lib.utexas.edu/maps/historical/newark_nj_1922.jpg", new LatLngBoundsLiteral()
        {
            North = 40.773941,
            South = 40.712216,
            East = -74.12544,
            West = -74.22655
        }, new GroundOverlayOptions()
        {
            Opacity = 0.5
        });

        await _mapoverlay.SetMap(map1.InteropObject);


    }


}