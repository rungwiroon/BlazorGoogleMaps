using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Polygon = GoogleMapsComponents.Maps.Polygon;

namespace Demo.Ui.Shared.Pages;

public partial class TestAllPage
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    private GoogleMap _map1 = default!;
    private MapOptions _mapOptions = default!;
    private readonly Stack<AdvancedMarkerElement> _markers = new Stack<AdvancedMarkerElement>();
    private MarkerClustering? _markerClustering;
    private readonly List<string> _events = [];
    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral(13.505892, 100.8162),
            MapTypeId = MapTypeId.Roadmap
        };
    }

    private Task AfterMapRender()
    {
        _events.Add("AfterMapRender trigered");
        return Task.CompletedTask;
    }

    private async Task StartTesting()
    {
        await JsRuntime.InvokeVoidAsync("alert", "Check console for errors");
        _events.Add("Testing started");
        _events.Add("Testing markers");
        await TestMarkers();
        _events.Add("Testing marker clustering");
        await InvokeClustering();
        _events.Add("Testing shapes");
        await TestShapes();
        _events.Add("Testing advanced marker");
        await TestAdvancedMarker();
        _events.Add("Testing events. Move and click on map!!!");
        await JsRuntime.InvokeVoidAsync("alert", "Move, drag and click on map!!!");
        await TestMapEvents();
    }

    private async Task TestMapEvents()
    {
        await _map1.InteropObject.AddListener("center_changed", OnCenter);
        await _map1.InteropObject.AddListener<MouseEvent>("click", OnClick);
        await _map1.InteropObject.AddListener("dblclick", OnDoubleClick);
        await _map1.InteropObject.AddListener("drag", OnDrag);

        return;

        void OnCenter()
        {
            AddEvent("Center changed");
        }

        void OnDoubleClick()
        {
            AddEvent("OnDoubleClicked ");
        }

        void OnDrag()
        {
            AddEvent("OnDrag");
        }

        void OnClick(MouseEvent x) => _events.Add($"OnDrag {x.LatLng}");
    }

    private void AddEvent(string eventName)
    {
        _events.Add(eventName);
        StateHasChanged();
    }

    private async Task TestAdvancedMarker()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();
        const string svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"56\" height=\"56\" viewBox=\"0 0 56 56\" fill=\"none\">\r\n  <rect width=\"56\" height=\"56\" rx=\"28\" fill=\"#7837FF\"></rect>\r\n  <path d=\"M46.0675 22.1319L44.0601 22.7843\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M11.9402 33.2201L9.93262 33.8723\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M27.9999 47.0046V44.8933\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M27.9999 9V11.1113\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M39.1583 43.3597L37.9186 41.6532\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M16.8419 12.6442L18.0816 14.3506\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M9.93262 22.1319L11.9402 22.7843\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M46.0676 33.8724L44.0601 33.2201\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M39.1583 12.6442L37.9186 14.3506\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M16.8419 43.3597L18.0816 41.6532\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M28 39L26.8725 37.9904C24.9292 36.226 23.325 34.7026 22.06 33.4202C20.795 32.1378 19.7867 30.9918 19.035 29.9823C18.2833 28.9727 17.7562 28.0587 17.4537 27.2401C17.1512 26.4216 17 25.5939 17 24.7572C17 23.1201 17.5546 21.7513 18.6638 20.6508C19.7729 19.5502 21.1433 19 22.775 19C23.82 19 24.7871 19.2456 25.6762 19.7367C26.5654 20.2278 27.34 20.9372 28 21.8649C28.77 20.8827 29.5858 20.1596 30.4475 19.6958C31.3092 19.2319 32.235 19 33.225 19C34.8567 19 36.2271 19.5502 37.3362 20.6508C38.4454 21.7513 39 23.1201 39 24.7572C39 25.5939 38.8488 26.4216 38.5463 27.2401C38.2438 28.0587 37.7167 28.9727 36.965 29.9823C36.2133 30.9918 35.205 32.1378 33.94 33.4202C32.675 34.7026 31.0708 36.226 29.1275 37.9904L28 39Z\" fill=\"#FF7878\"></path>\r\n</svg>";

        var advancedMarker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            Content = svg
        });

        var position = await advancedMarker.GetPosition();
        _events.Add("Advanced marker position: " + position);
        if (position.Lat == 0 || Math.Abs(mapCenter.Lat - position.Lat) > 0.0001)
        {
            throw new Exception("Advanced marker position is not set correctly. Please check the console for errors.");
        }

        if (position.Lng == 0 || Math.Abs(mapCenter.Lng - position.Lng) > 0.0001)
        {
            throw new Exception("Advanced marker position is not set correctly. Please check the console for errors.");
        }

    }

    private async Task TestShapes()
    {
        var polyline = await Polyline.CreateAsync(_map1.JsRuntime, new PolylineOptions()
        {
            Draggable = true,
            Editable = true,
            Map = _map1.InteropObject
        });

        await polyline.SetPath([
            await GeRandomLatLng(),
            await GeRandomLatLng(),
            await GeRandomLatLng()
        ]);

        var polygon = await Polygon.CreateAsync(_map1.JsRuntime, new PolygonOptions()
        {
            Draggable = false,
            Editable = true,
            Map = _map1.InteropObject,
        });

        await polygon.SetPath([
            await GeRandomLatLng(),
            await GeRandomLatLng(),
            await GeRandomLatLng()
        ]);

        var bounds = new LatLngBoundsLiteral(await GeRandomLatLng(), await GeRandomLatLng());
        _ = await Rectangle.CreateAsync(_map1.JsRuntime, new RectangleOptions()
        {
            Draggable = true,
            Editable = true,
            Map = _map1.InteropObject,
            Bounds = bounds
        });

        _ = await Circle.CreateAsync(_map1.JsRuntime, new CircleOptions()
        {
            Draggable = true,
            Editable = true,
            Map = _map1.InteropObject,
            Radius = 1000,
            Center = await GeRandomLatLng()
        });
    }

    private async Task TestMarkers()
    {
        var labelText = $"Test {_markers.Count}";
        var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = await GeRandomLatLng(),
            Map = _map1.InteropObject,
            ZIndex = 10,
            Title = labelText,
            GmpClickable = true,
            Content = new PinElement
            {
                Glyph = labelText
            }
        });

        _markers.Push(marker);
    }

    private async Task InvokeClustering()
    {
        if (_markerClustering != null)
        {
            await _markerClustering.ClearMarkers();
        }

        var coordinates = GetClusterCoordinates();

        var markers = await GetMarkers(coordinates, _map1.InteropObject);

        if (_markerClustering == null)
        {
            // If adding a clustering event listener, initialize markerclusering with an empty marker list 
            // Clustering happens immediately upon adding markers, so including markers with the init 
            // creates a race condition with JSInterop adding a listener. If not adding a listener, pass markers
            // to CreateAsync to eliminate the latency of a second JSInterop call to AddMarkers.
            _markerClustering = await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, new List<AdvancedMarkerElement>(), new MarkerClustererOptions()
            {
                // RendererObjectName = "customRendererLib.interpolatedRenderer"
            });
        }
        await _markerClustering.AddMarkers(markers);

        var boundsLiteral = new LatLngBoundsLiteral(coordinates.First());
        foreach (var literal in coordinates)
        {
            LatLngBoundsLiteral.CreateOrExtend(ref boundsLiteral, literal);
        }

        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, GoogleMapsComponents.Maps.Coordinates.Padding>.FromT0(1));

    }

    private async Task<IEnumerable<AdvancedMarkerElement>> GetMarkers(ICollection<LatLngLiteral> coords, Map map)
    {
        var result = new List<AdvancedMarkerElement>(coords.Count());
        var index = 1;
        foreach (var latLngLiteral in coords)
        {
            var labelText = $"Test {index++}";
            var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = latLngLiteral,
                Map = map,
                Title = labelText,
                GmpClickable = true,
                Content = new PinElement
                {
                    Glyph = labelText
                }
            });

            result.Add(marker);
        }


        return result;
    }

    private static List<LatLngLiteral> GetClusterCoordinates()
    {
        return
        [
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
            new(-43.999792, 170.463352)
        ];
    }

    private async Task<LatLngLiteral> GeRandomLatLng()
    {
        var bounds = await _map1.InteropObject.GetBounds();
        //var lngSpan = bounds.East - bounds.West;
        //var latSpan = bounds.North - bounds.South;

        //var marker = new LatLngLiteral()
        //{

        //    Lat = bounds.South + latSpan * new Random().NextDouble(),
        //    Lng = bounds.West + lngSpan * new Random().NextDouble()

        //};

        //return marker;

        var rnd = new Random();

        return new LatLngLiteral(bounds.South + rnd.NextDouble() * (bounds.North - bounds.South), bounds.West + rnd.NextDouble() * (bounds.East - bounds.West));
    }
}
