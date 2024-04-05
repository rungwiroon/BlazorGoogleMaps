using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapCircleListPage : ComponentBase
{
    private GoogleMap _map = null!;
    private MapOptions _mapOptions = null!;
    private int _bunchsize = 10;

    private CircleList? _circleList;
    private readonly Dictionary<string, CircleOptions> _circleOptionsByRef = new Dictionary<string, CircleOptions>();
    private int _lastId;
    private PolygonList? _createedPolygons;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral()
            {
                Lat = 48.994249,
                Lng = 12.190451
            },
            MapTypeId = MapTypeId.Roadmap
        };
    }

    /// <summary>
    /// Create a bunch of circles, put them into a dictionary with reference ids and display them on the map.
    /// </summary>
    private async void CreateBunchOfPolygon()
    {
        var outerCoords = new List<LatLngLiteral>()
        {
            new LatLngLiteral(13.501908279929077, 100.69801114196777),
            new LatLngLiteral(13.491392275719202, 100.74933789367675),
            new LatLngLiteral(13.465851481053091, 100.71637890930175),
        };

        var innerCoords = new List<LatLngLiteral>()
        {
            new LatLngLiteral(13.487386057049033, 100.72633526916503),
            new LatLngLiteral(13.48137660307361, 100.719125491333),
            new LatLngLiteral(13.478705686132331, 100.72959683532714),
        };

        _createedPolygons = await PolygonList.CreateAsync(_map.JsRuntime, new Dictionary<string, PolygonOptions>()
        {
            { Guid.NewGuid().ToString(), new PolygonOptions()
            {
                Paths = new[] { outerCoords, innerCoords },
                Draggable = true,
                Editable = false,
                FillColor = "blue",
                ZIndex = 999,
                Visible = true,
                StrokeWeight = 5,
                Map = _map.InteropObject
            }}
        });
        var first = _createedPolygons.Polygons.First().Value;
        var path = await first.GetPath();
        await _map.InteropObject.SetCenter(path.First());
    }

    private async void CreateBunchOfCircles()
    {
        int howMany = _bunchsize;
        var bounds = await _map.InteropObject.GetBounds();
        double maxRadius = (bounds.North - bounds.South) * 111111.0 / (10 + Math.Sqrt(howMany));
        var colors = new[] { "#FFFFFF", "#9132D1", "#FFD800", "#846A00", "#AAC643", "#C96A00", "#B200FF", "#CD6A00", "#00A321", "#7F6420" };
        var rnd = new Random();
        for (int i = 0; i < howMany; i++)
        {
            var color = colors[rnd.Next(0, colors.Length)];
            var circleOptions = new CircleOptions
            {
                Map = _map.InteropObject,
                Center = new LatLngLiteral
                {
                    Lat = bounds.South + rnd.NextDouble() * (bounds.North - bounds.South),
                    Lng = bounds.West + rnd.NextDouble() * (bounds.East - bounds.West)
                },
                Radius = (rnd.NextDouble() + 0.2) / 1.2 * maxRadius,
                StrokeColor = color,
                StrokeOpacity = 0.60f,
                StrokeWeight = 2,
                FillColor = color,
                FillOpacity = 0.35f,
                Visible = true,
                ZIndex = 1000000,
            };
            _circleOptionsByRef[(++_lastId).ToString()] = circleOptions;
        }

        await RefreshCircleList();
    }

    private async Task RefreshCircleList()
    {
        _circleList = await CircleList.SyncAsync(_circleList, _map.JsRuntime, _circleOptionsByRef, async (ev, sKey, entity) =>
        {
            // Circle has been clicked --> delete it.
            _circleOptionsByRef.Remove(sKey);
            await RefreshCircleList();
        });
    }

    private async Task RemoveBunchOfPolygon()
    {
        if (_createedPolygons != null)
        {
            foreach (var markerListMarker in _createedPolygons.Polygons)
            {
                await markerListMarker.Value.SetMap(null);
            }

            await _createedPolygons.RemoveAllAsync();
        }
    }
}