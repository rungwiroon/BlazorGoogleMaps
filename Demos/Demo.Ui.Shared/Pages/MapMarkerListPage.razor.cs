using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Ui.Shared.Pages;

public partial class MapMarkerListPage
{
    private GoogleMap _map1;

    private MapOptions _mapOptions;

    private Stack<AdvancedMarkerElement> _markers = new Stack<AdvancedMarkerElement>();

    private LatLngBounds _bounds;
    private AdvancedMarkerElementList? _markerList;

    [Inject]
    public IJSRuntime JsObjectRef { get; set; } = null!;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral(13.505892, 100.8162),
            MapTypeId = MapTypeId.Roadmap
        };
    }

    protected async Task OnAfterInit()
    {
        _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
    }

    private async Task InvokeClustering()
    {
        var coordinates = new List<LatLngLiteral>()
        {
            new(-31.56391, 147.154312),
            new(-33.718234, 150.363181),
            new(-33.727111, 150.371124),
            new(-33.848588, 151.209834),
            new(-33.851702, 151.216968),
            new(-34.671264, 150.863657),
            new(-35.304724, 148.662905),
            new(-36.817685, 175.699196),
            new(-36.828611, 175.790222),
            new(-37.75, 145.116667),
            new(-37.759859, 145.128708),
            new(-37.765015, 145.133858),
            new(-37.770104, 145.143299),
            new(-37.7737, 145.145187),
            new(-37.774785, 145.137978),
            new(-37.819616, 144.968119),
            new(-38.330766, 144.695692),
            new(-39.927193, 175.053218),
            new(-41.330162, 174.865694),
            new(-42.734358, 147.439506),
            new(-42.734358, 147.501315),
            new(-42.735258, 147.438),
            new(-43.999792, 170.463352),
        };

        var markers = await GetMarkers(coordinates, _map1.InteropObject);

        await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, markers);

        //initMap
        //await JsObjectRef.InvokeAsync<object>("initMap", map1.InteropObject.Guid.ToString(), markers);
    }

    private async Task<IEnumerable<AdvancedMarkerElement>> GetMarkers(IReadOnlyList<LatLngLiteral> coords, Map map)
    {
        var result = new List<AdvancedMarkerElement>(coords.Count());
        var index = 1;

        foreach (var latLngLiteral in coords)
        {
            var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = latLngLiteral,
                Map = map,
                Title = $"Test {index}",
                GmpClickable = true,
                Content = new PinElement
                {
                    Glyph = $"Test {index++}"
                }
            });

            result.Add(marker);
        }


        return result;
    }

    private async Task AddMarker2()
    {
        var coordinates = new List<LatLngLiteral>()
        {
            new(-37.759859, 145.128708),
            new(-37.765015, 145.133858),
            new(-37.770104, 145.143299),
            new(-37.7737, 145.145187),
            new(-37.774785, 145.137978),
            new(-37.819616, 144.968119),
            new(-38.330766, 144.695692),
            new(-39.927193, 175.053218),
            new(-41.330162, 174.865694),
            new(-42.734358, 147.439506),
            new(-42.734358, 147.501315),
            new(-42.735258, 147.438),
            new(-43.999792, 170.463352),
        };
        await AddMarkersGroup(coordinates);
    }

    private async Task AddMarker1()
    {
        var coordinates = new List<LatLngLiteral>()
        {
            new(-31.56391, 147.154312),
            new(-33.718234, 150.363181),
            new(-33.727111, 150.371124),
            new(-33.848588, 151.209834),
            new(-33.851702, 151.216968),
            new(-34.671264, 150.863657),
            new(-35.304724, 148.662905),
            new(-36.817685, 175.699196),
            new(-36.828611, 175.790222),
            new(-37.75, 145.116667),
        };


        for (int index = 0; index < 200; index++)
        {
            var dif = (index * 0.001);
            coordinates.Add(new LatLngLiteral(-37.75 + dif, 145.116667 + dif));
        }

        await AddMarkersGroup(coordinates);
    }



    private async Task AddMarkersGroup(IReadOnlyList<LatLngLiteral> coordinates)
    {
        if (_markerList == null)
        {
            _markerList = await AdvancedMarkerElementList.CreateAsync(
                _map1.JsRuntime,
                coordinates.ToDictionary(s => Guid.NewGuid().ToString(), y => new AdvancedMarkerElementOptions()
                {
                    Position = y,
                    Map = _map1.InteropObject,
                    GmpClickable = true,
                    Title = Guid.NewGuid().ToString(),
                    Content = new PinElement()
                })
            );
        }
        else
        {
            var cordDic = coordinates.ToDictionary(s => Guid.NewGuid().ToString(), y => new AdvancedMarkerElementOptions()
            {
                Position = y,
                Map = _map1.InteropObject,
                GmpClickable = true,
                Title = Guid.NewGuid().ToString(),
                Content = new PinElement()
            });

            await _markerList.AddMultipleAsync(cordDic);
        }


        foreach (var latLngLiteral in coordinates)
        {
            await _bounds.Extend(latLngLiteral);
        }


        await FitBounds();
    }

    private async Task RemoveMarkers()
    {
        foreach (var markerListMarker in _markerList.Markers)
        {
            await markerListMarker.Value.SetMap(null);
        }

        await _markerList.RemoveAllAsync();
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
        if (await _bounds.IsEmpty())
        {
            return;
        }

        var boundsLiteral = await _bounds.ToJson();
        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, Padding>.FromT0(5));
    }
}
