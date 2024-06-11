using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapAdvancedMarkerElementListPage
{
    private GoogleMap _map1;

    private MapOptions _mapOptions;

    private Stack<Marker> _markers = new Stack<Marker>();

    private LatLngBounds _bounds;

    private AdvancedMarkerElementList? _markerElementList;

    [Inject]
    public IJSRuntime JsObjectRef { get; set; } = default!;

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
            MapTypeId = MapTypeId.Roadmap,
            MapId = "map1",
        };
    }

    protected async Task OnAfterInit()
    {
        _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
    }

    private async Task AddMarker2()
    {
        var coordinates = new List<LatLngLiteral>()
        {
            new LatLngLiteral(){ Lng = 145.128708, Lat = -37.759859 },
            new LatLngLiteral(){ Lng = 145.133858, Lat = -37.765015 },
            new LatLngLiteral(){ Lng = 145.143299, Lat = -37.770104 },
            new LatLngLiteral(){ Lng = 145.145187, Lat = -37.7737 },
            new LatLngLiteral(){ Lng = 145.137978, Lat = -37.774785 },
            new LatLngLiteral(){ Lng = 144.968119, Lat = -37.819616 },
            new LatLngLiteral(){ Lng = 144.695692, Lat = -38.330766 },
            new LatLngLiteral(){ Lng = 175.053218, Lat = -39.927193 },
            new LatLngLiteral(){ Lng = 174.865694, Lat = -41.330162 },
            new LatLngLiteral(){ Lng = 147.439506, Lat = -42.734358 },
            new LatLngLiteral(){ Lng = 147.501315, Lat = -42.734358 },
            new LatLngLiteral(){ Lng = 147.438, Lat = -42.735258 },
            new LatLngLiteral(){ Lng = 170.463352, Lat = -43.999792 },
        };
        await AddMarkersGroup(coordinates);
    }

    private async Task AddMarker1()
    {
        var coordinates = new List<LatLngLiteral>()
        {
            new LatLngLiteral(){ Lng = 147.154312, Lat = -31.56391 },
            new LatLngLiteral(){ Lng = 150.363181, Lat = -33.718234 },
            new LatLngLiteral(){ Lng = 150.371124, Lat = -33.727111 },
            new LatLngLiteral(){ Lng = 151.209834, Lat = -33.848588 },
            new LatLngLiteral(){ Lng = 151.216968, Lat = -33.851702 },
            new LatLngLiteral(){ Lng = 150.863657, Lat = -34.671264 },
            new LatLngLiteral(){ Lng = 148.662905, Lat = -35.304724 },
            new LatLngLiteral(){ Lng = 175.699196, Lat = -36.817685 },
            new LatLngLiteral(){ Lng = 175.790222, Lat = -36.828611 },
            new LatLngLiteral(){ Lng = 145.116667, Lat = -37.75 },
        };


        for (int index = 0; index < 200; index++)
        {
            var dif = (index * 0.001);
            coordinates.Add(new LatLngLiteral() { Lng = 145.116667 + dif, Lat = -37.75 + dif });
        }

        await AddMarkersGroup(coordinates);
    }

    private async Task AddMarkersGroup(ICollection<LatLngLiteral> coordinates)
    {
        if (_markerElementList == null)
        {
            _markerElementList = await AdvancedMarkerElementList.CreateAsync(
                _map1.JsRuntime,
                coordinates.ToDictionary(_ => Guid.NewGuid().ToString(), y => new AdvancedMarkerElementOptions()
                {
                    Position = new LatLngLiteral() { Lng = y.Lng, Lat = y.Lat },
                    Map = _map1.InteropObject,
                    //Icon = new Icon() { Url = s.MarkerIconPath, ScaledSize = iconSize, Anchor = iconAnchor },
                    GmpDraggable = true,
                    Title = Guid.NewGuid().ToString(),
                })
            );
        }
        else
        {
            var cordDic = coordinates.ToDictionary(_ => Guid.NewGuid().ToString(), y => new AdvancedMarkerElementOptions()
            {
                Position = new LatLngLiteral() { Lng = y.Lng, Lat = y.Lat },
                Map = _map1.InteropObject,
                //Icon = new Icon() { Url = s.MarkerIconPath, ScaledSize = iconSize, Anchor = iconAnchor },
                GmpDraggable = true,
                Title = Guid.NewGuid().ToString(),
            });

            await _markerElementList.AddMultipleAsync(cordDic);
        }

        foreach (var latLngLiteral in coordinates)
        {
            await _bounds.Extend(latLngLiteral);
        }

        await FitBounds();
    }

    private async Task RemoveMarkers()
    {
        if (_markerElementList == null)
        {
            return;
        }

        foreach (var markerListMarker in _markerElementList.Markers)
        {
            await markerListMarker.Value.SetMap(null);
        }

        await _markerElementList.RemoveAllAsync();
    }

    private async Task RemoveMarker()
    {
        if (!_markers.Any())
        {
            return;
        }

        var lastMarker = _markers.Pop();
        await lastMarker.SetMap(null);
    }

    private async Task Recenter()
    {
        if (!_markers.Any())
        {
            return;
        }

        var lastMarker = _markers.Peek();
        var center = await _map1.InteropObject.GetCenter();
        await lastMarker.SetPosition(center);
    }

    private async Task FitBounds()
    {
        if (await this._bounds.IsEmpty())
        {
            return;
        }

        var boundsLiteral = await _bounds.ToJson();
        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, Padding>.FromT0(5));
    }
}