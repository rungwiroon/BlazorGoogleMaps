using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapGroundOverlayPage
{
    private GoogleMap _map1;
    private MapOptions _mapOptions;

    private readonly List<String> _events = new List<String>();

    private GroundOverlay _mapoverlay;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
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

    private async Task RemoveOverlay()
    {
        await _mapoverlay.SetMap(null);
    }
    private async Task AddOverlay()
    {
        _mapoverlay = await GroundOverlay.CreateAsync(_map1.JsRuntime, "https://www.lib.utexas.edu/maps/historical/newark_nj_1922.jpg", new LatLngBoundsLiteral()
        {
            North = 40.773941,
            South = 40.712216,
            East = -74.12544,
            West = -74.22655
        }, new GroundOverlayOptions()
        {
            Opacity = 0.5
        });

        await _mapoverlay.SetMap(_map1.InteropObject);
    }
}