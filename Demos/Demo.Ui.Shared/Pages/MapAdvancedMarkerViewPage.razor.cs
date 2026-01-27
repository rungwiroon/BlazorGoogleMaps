using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Ui.Shared.Pages;

public sealed partial class MapAdvancedMarkerViewPage
{
    private GoogleMap _map1 = null!;
    private int _counter;
    private const string Svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"56\" height=\"56\" viewBox=\"0 0 56 56\" fill=\"none\">\r\n  <rect width=\"56\" height=\"56\" rx=\"28\" fill=\"#7837FF\"></rect>\r\n  <path d=\"M46.0675 22.1319L44.0601 22.7843\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M11.9402 33.2201L9.93262 33.8723\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M27.9999 47.0046V44.8933\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M27.9999 9V11.1113\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M39.1583 43.3597L37.9186 41.6532\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M16.8419 12.6442L18.0816 14.3506\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M9.93262 22.1319L11.9402 22.7843\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M46.0676 33.8724L44.0601 33.2201\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M39.1583 12.6442L37.9186 14.3506\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M16.8419 43.3597L18.0816 41.6532\" stroke=\"white\" stroke-width=\"2.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></path>\r\n  <path d=\"M28 39L26.8725 37.9904C24.9292 36.226 23.325 34.7026 22.06 33.4202C20.795 32.1378 19.7867 30.9918 19.035 29.9823C18.2833 28.9727 17.7562 28.0587 17.4537 27.2401C17.1512 26.4216 17 25.5939 17 24.7572C17 23.1201 17.5546 21.7513 18.6638 20.6508C19.7729 19.5502 21.1433 19 22.775 19C23.82 19 24.7871 19.2456 25.6762 19.7367C26.5654 20.2278 27.34 20.9372 28 21.8649C28.77 20.8827 29.5858 20.1596 30.4475 19.6958C31.3092 19.2319 32.235 19 33.225 19C34.8567 19 36.2271 19.5502 37.3362 20.6508C38.4454 21.7513 39 23.1201 39 24.7572C39 25.5939 38.8488 26.4216 38.5463 27.2401C38.2438 28.0587 37.7167 28.9727 36.965 29.9823C36.2133 30.9918 35.205 32.1378 33.94 33.4202C32.675 34.7026 31.0708 36.226 29.1275 37.9904L28 39Z\" fill=\"#FF7878\"></path>\r\n</svg>";
    private readonly MapOptions _mapOptions = new MapOptions()
    {
        Zoom = 13,
        Center = new LatLngLiteral(13.505892, 100.8162),
        IsFractionalZoomEnabled = false,
        HeadingInteractionEnabled = false,
        CameraControl = true,
        MapTypeId = MapTypeId.Roadmap,
        // ColorScheme = ColorScheme.Dark,
        RenderingType = RenderingType.Vector,
        ZoomControl = true,
        ZoomControlOptions = new ZoomControlOptions()
        {
            Position = ControlPosition.BottomCenter,
        },
    };
    private LatLngBounds _bounds = null!;
    private readonly List<String> _events = new List<String>();
    private readonly Stack<AdvancedMarkerElement> _advancedMarkerElements = new Stack<AdvancedMarkerElement>();
    private AdvancedMarkerElementList? _markerElementList;
    private MarkerClustering? _markerClustering;
    private IEnumerable<AdvancedMarkerElement>? _clusteringMarkers;

    [Inject]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IJSRuntime JsObjectRef { get; set; } = null!;

    private async Task OnAfterRenderAsync()
    {
        _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
    }

    private async Task AddMarker()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();

        var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            Content = Svg
        });

        _advancedMarkerElements.Push(marker);
        //await marker.SetZIndex(15);
        await _bounds.Extend(mapCenter);

        await marker.AddListener<MouseEvent>("click", e =>
        {
            _events.Add($"Clicked {e.LatLng.Lat} {e.LatLng.Lng}");
            StateHasChanged();
            e.Stop();
        });
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


        for (var index = 0; index < 200; index++)
        {
            var dif = (index * 0.001);
            coordinates.Add(new LatLngLiteral(-37.75 + dif, 145.116667 + dif));
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
                    Position = y,
                    Map = _map1.InteropObject,
                    GmpDraggable = true,
                    Title = Guid.NewGuid().ToString(),
                    Content = "<div style='background-color:blue'>My pin</div>",
                })
            );
        }
        else
        {
            var cordDic = coordinates.ToDictionary(_ => Guid.NewGuid().ToString(), y => new AdvancedMarkerElementOptions()
            {
                Position = y,
                Map = _map1.InteropObject,
                GmpDraggable = true,
                Title = Guid.NewGuid().ToString(),
                Content = "<div style='background-color:blue'>My pin</div>",
            });

            await _markerElementList.AddMultipleAsync(cordDic);
        }

        foreach (var latLngLiteral in coordinates)
        {
            await _bounds.Extend(latLngLiteral);
        }

        await FitBounds();

        await AdvancedMarkerElementList.SyncAsync(
            _markerElementList,
            _map1.JsRuntime,
            new Dictionary<string, AdvancedMarkerElementOptions>());
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
        if (!_advancedMarkerElements.Any())
        {
            return;
        }

        var lastMarker = _advancedMarkerElements.Pop();
        await lastMarker.SetMap(null);
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

    private async Task AddMarkerWithPinElement()
    {
        //Some examples of pin elements
        //https://developers.google.com/maps/documentation/javascript/advanced-markers/graphic-markers#javascript
        var mapCenter = await _map1.InteropObject.GetCenter();
        PinElement? pinElement;
        _counter++;
        const string constantImageSource = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png";

        AdvancedMarkerElement marker;
        var actionNr = _counter % 3;
        if (actionNr == 0)
        {
            pinElement = new PinElement()
            {
                BorderColor = "red",
                Background = "blue",
                Scale = 1.99
            };

            marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = mapCenter,
                Map = _map1.InteropObject,
                Content = pinElement
            });
        }
        else if (actionNr == 1)
        {
            pinElement = new PinElement()
            {
                Glyph = constantImageSource,
                Background = "green",
                Scale = 2.5
            };

            marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = mapCenter,
                Map = _map1.InteropObject,
                Content = pinElement

            });
        }
        else
        {
            marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = mapCenter,
                Map = _map1.InteropObject,
                Content = Svg
            });
        }

        _advancedMarkerElements.Push(marker);
    }

    private async Task UpdatePosition()
    {
        if (!_advancedMarkerElements.Any())
        {
            return;
        }

        var lastMarker = _advancedMarkerElements.Peek();
        var mapCenter = await _map1.InteropObject.GetCenter();
        await lastMarker.SetPosition(mapCenter);
        await _bounds.Extend(mapCenter);
    }

    private async Task UpdateContent()
    {
        if (!_advancedMarkerElements.Any())
        {
            return;
        }

        const string newContent = "<svg width=\"200\" height=\"200\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <!-- Body -->\r\n  <ellipse cx=\"100\" cy=\"130\" rx=\"50\" ry=\"30\" fill=\"blue\"/>\r\n  \r\n  <!-- Wing -->\r\n  <ellipse cx=\"80\" cy=\"120\" rx=\"30\" ry=\"20\" fill=\"lightblue\"/>\r\n  \r\n  <!-- Head -->\r\n  <circle cx=\"100\" cy=\"80\" r=\"25\" fill=\"blue\"/>\r\n  \r\n  <!-- Eye -->\r\n  <circle cx=\"110\" cy=\"75\" r=\"5\" fill=\"black\"/>\r\n  \r\n  <!-- Beak -->\r\n  <polygon points=\"125,80 135,75 125,70\" fill=\"orange\"/>\r\n  \r\n  <!-- Tail -->\r\n  <polygon points=\"60,130 50,140 70,140\" fill=\"blue\"/>\r\n  \r\n  <!-- Legs -->\r\n  <line x1=\"90\" y1=\"160\" x2=\"90\" y2=\"180\" stroke=\"orange\" stroke-width=\"3\"/>\r\n  <line x1=\"110\" y1=\"160\" x2=\"110\" y2=\"180\" stroke=\"orange\" stroke-width=\"3\"/>\r\n</svg>\r\n";
        var isPin = _counter % 2 == 0;
        if (isPin)
        {
            var pinElement = new PinElement()
            {
                BorderColor = "red",
                Background = "blue",
                Scale = 2
            };
            var lastMarker = _advancedMarkerElements.Peek();
            await lastMarker.SetContent(pinElement);
        }
        else
        {
            var lastMarker = _advancedMarkerElements.Peek();
            await lastMarker.SetContent(newContent);
        }

        _counter++;
    }

    private async Task GetMarkerPosition()
    {
        if (!_advancedMarkerElements.Any())
        {
            return;
        }

        var lastMarker = _advancedMarkerElements.Peek();
        var position = await lastMarker.GetPosition();
        _events.Add($"Marker position {position.Lat:F} {position.Lng:F} Alt {position.Altitude}");

        var gmpClickable = await lastMarker.GetGmpClickable();
        await lastMarker.SetGmpClickable(!gmpClickable);
        var newGpmClickable = await lastMarker.GetGmpClickable();
        _events.Add($"gmpClickable new and old value are equal: {newGpmClickable == !gmpClickable}");

        var gmpDraggable = await lastMarker.GetGmpDraggable();
        await lastMarker.SetGmpDraggable(!gmpDraggable);
        var newGmpDraggable = await lastMarker.GetGmpDraggable();
        _events.Add($"gmpDraggable new and old value are equal: {newGmpDraggable == !gmpDraggable}");

        StateHasChanged();
    }

    private async Task InvokeClustering()
    {
        var coordinates = GetClusterCoordinates();

        _clusteringMarkers = await GetMarkers(coordinates);

        _markerClustering = await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, _clusteringMarkers, new()
        {
            //RendererObjectName = "customRendererLib.interpolatedRenderer",
            ZoomOnClick = true,
        });


        foreach (var latLngLiteral in coordinates)
        {
            await _bounds.Extend(latLngLiteral);
        }

        var boundsLiteral = await _bounds.ToJson();
        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, Padding>.FromT0(5));
    }

    private async Task<IEnumerable<AdvancedMarkerElement>> GetMarkers(IReadOnlyList<LatLngLiteral> coords)
    {
        var result = new List<AdvancedMarkerElement>(coords.Count());
        var index = 1;
        foreach (var latLngLiteral in coords)
        {
            var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
            {
                Position = latLngLiteral,
                Map = _map1.InteropObject,
                Content = index.ToString()
            });

            result.Add(marker);
        }


        return result;
    }

    private static List<LatLngLiteral> GetClusterCoordinates()
    {
        return new List<LatLngLiteral>()
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
    }

    private async Task ClearClustering()
    {
        if (_markerClustering == null || _clusteringMarkers == null)
        {
            return;
        }

        await _markerClustering.RemoveMarkers(_clusteringMarkers);
    }
}
