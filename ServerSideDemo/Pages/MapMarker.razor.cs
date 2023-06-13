using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ServerSideDemo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class MapMarker
{
    private GoogleMap _map1;
    private MapOptions _mapOptions;

    private readonly Stack<Marker> _markers = new Stack<Marker>();
    private readonly List<String> _events = new List<String>();

    private MapEventList _eventList;

    private LatLngBounds _bounds;
    private MarkerClustering _markerClustering;
    public int ZIndex { get; set; } = 0;

    [Inject]
    public IJSRuntime JsObjectRef { get; set; }

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
            MapId = "3a3b33f0edd6ed2a"
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
        }
    }

    private async Task ClearClustering()
    {
        if (_markerClustering != null)
            await _markerClustering.ClearMarkers();
    }

    private async Task InvokeClustering()
    {
        await ClearClustering();
        var coordinates = GetClusterCoordinates();

        var markers = await GetMarkers(coordinates, _map1.InteropObject);

        if (_markerClustering == null)
        {
            // If adding a clustering event listener, initialize markerclusering with an empty marker list 
            // Clustering happens immediately upon adding markers, so including markers with the init 
            // creates a race condition with JSInterop adding a listener. If not adding a listener, pass markers
            // to CreateAsync to eliminate the latency of a second JSInterop call to AddMarkers.
            _markerClustering = await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, new List<Marker>(), new MarkerClustererOptions()
            {
                // RendererObjectName = "customRendererLib.interpolatedRenderer"
            });
            await _markerClustering.AddListener("clusteringend", async () => { await SetMarkerListeners(); });
        }
        await _markerClustering.AddMarkers(markers);

        LatLngBoundsLiteral boundsLiteral = new LatLngBoundsLiteral(new LatLngLiteral() { Lat = coordinates.First().Lat, Lng = coordinates.First().Lng });
        foreach (var literal in coordinates)
            LatLngBoundsLiteral.CreateOrExtend(ref boundsLiteral, literal);
        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, GoogleMapsComponents.Maps.Coordinates.Padding>.FromT0(1));

    }

    private List<string>? _listeningLoneMarkerKeys;
    private async Task SetMarkerListeners()
    {
        if (_listeningLoneMarkerKeys == null)
            _listeningLoneMarkerKeys = new List<string>();

        //use GetMappedValue<T> to map and extract the array of guid keys of unclustered markers
        GoogleMapsComponents.JsObjectRef jsRef = new GoogleMapsComponents.JsObjectRef(JsObjectRef, _markerClustering.Guid);
        var guidStrings = (await jsRef.GetMappedValue<List<string>>("clusters", "marker", "guidString"))
            .Where((x) => { return x != null; });

        if (!guidStrings.Any())
            return;

        // Among markers not in clusters, find those which don't yet have a listener
        MarkerList deafLoneMarkersList = await MarkerList.CreateAsync(JsObjectRef, new Dictionary<string, MarkerOptions>());
        foreach (var key in guidStrings)
        {
            var markr = _markers.First(x => key == x.Guid.ToString());
            if (_listeningLoneMarkerKeys.Contains(key))
                continue;
            deafLoneMarkersList.BaseListableEntities.Add(key, markr);
            _listeningLoneMarkerKeys.Add(key);
        }

        if (!deafLoneMarkersList.BaseListableEntities.Any())
            return;

        await deafLoneMarkersList.AddListeners<MouseEvent>(deafLoneMarkersList.Markers.Keys.ToList(), "click", async (o, e) =>
        {
            //await JsObjectRef.InvokeVoidAsync("loneMarkerClickEvent", e);
        });

        // if all points set, clean up idle listener.
        if (_listeningLoneMarkerKeys.Count == _markers.Count)
        {
            _listeningLoneMarkerKeys = null;
            await _markerClustering.ClearListeners("clusteringend");
        }
    }

    private async Task InvokeStyledIconsClustering()
    {
        await ClearClustering();
        var coordinates = GetClusterCoordinates();

        var markers = await GetMarkers(coordinates, _map1.InteropObject);

        _markerClustering = await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, markers, new()
        {
            RendererObjectName = "customRendererLib.interpolatedRenderer",
            ZoomOnClick = true,
        });

        await _markerClustering.FitMapToMarkers(1);
        //initMap
        //await JsObjectRef.InvokeAsync<object>("initMap", map1.InteropObject.Guid.ToString(), markers);
    }

    private static List<LatLngLiteral> GetClusterCoordinates()
    {
        return new List<LatLngLiteral>()
        {
            new() { Lng = 147.154312, Lat = -31.56391},
            new() { Lng = 150.363181, Lat = -33.718234},
            new() { Lng = 150.371124, Lat = -33.727111},
            new() { Lng = 151.209834, Lat = -33.848588},
            new() { Lng = 151.216968, Lat = -33.851702},
            new() { Lng = 150.863657, Lat = -34.671264},
            new() { Lng = 148.662905, Lat = -35.304724},
            new() { Lng = 175.699196, Lat = -36.817685},
            new() { Lng = 175.790222, Lat = -36.828611},
            new() { Lng = 145.116667, Lat = -37.75},
            new() { Lng = 145.128708, Lat = -37.759859},
            new() { Lng = 145.133858, Lat = -37.765015},
            new() { Lng = 145.143299, Lat = -37.770104},
            new() { Lng = 145.145187, Lat = -37.7737},
            new() { Lng = 145.137978, Lat = -37.774785},
            new() { Lng = 144.968119, Lat = -37.819616},
            new() { Lng = 144.695692, Lat = -38.330766},
            new() { Lng = 175.053218, Lat = -39.927193},
            new() { Lng = 174.865694, Lat = -41.330162},
            new() { Lng = 147.439506, Lat = -42.734358},
            new() { Lng = 147.501315, Lat = -42.734358},
            new() { Lng = 147.438, Lat = -42.735258},
            new() { Lng = 170.463352, Lat = -43.999792},
        };
    }

    private async Task<IEnumerable<Marker>> GetMarkers(IEnumerable<LatLngLiteral> coords, Map map)
    {
        var result = new List<Marker>(coords.Count());
        var index = 1;
        foreach (var latLngLiteral in coords)
        {
            var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions()
            {
                Position = latLngLiteral,
                Map = map,
                Label = $"Test {index++}",

                //Icon = new Icon()
                //{
                //    Url = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
                //}
                //Icon = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
            });

            result.Add(marker);
        }


        return result;
    }

    private async Task AddMarkerStyled()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();
        ZIndex++;

        var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            ZIndex = ZIndex,
            //Icon = new Symbol()
            //{
            //    Path = "M10.453 14.016l6.563-6.609-1.406-1.406-5.156 5.203-2.063-2.109-1.406 1.406zM12 2.016q2.906 0 4.945 2.039t2.039 4.945q0 1.453-0.727 3.328t-1.758 3.516-2.039 3.070-1.711 2.273l-0.75 0.797q-0.281-0.328-0.75-0.867t-1.688-2.156-2.133-3.141-1.664-3.445-0.75-3.375q0-2.906 2.039-4.945t4.945-2.039z"
            //},
            //Note that font properties are overriden in class
            //Please be cautious about versioning issues and some issues when using other tools
            //https://developers.google.com/maps/documentation/javascript/reference/marker#MarkerLabel.className
            Label = new MarkerLabel
            {
                Text = $"Test {_markers.Count()}",
                FontWeight = "bold",
                Color = "#5B32FF",
                FontSize = "24",
                ClassName = "map-marker-label",

            },
        });

        _markers.Push(marker);

        return;
    }
    private async Task AddMarker()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();
        ZIndex++;

        var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            //Label = $"Test {markers.Count}",
            ZIndex = ZIndex,
            //CollisionBehavior = CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY,//2021-07 supported only in beta google maps version
            //Animation = Animation.Bounce
            //Icon = new Icon()
            //{
            //    Url = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
            //}
            //Icon = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
        });

        _markers.Push(marker);

        //return;
        await _bounds.Extend(mapCenter);

        var icon = await marker.GetIcon();

        Console.WriteLine($"Get icon result type is : {icon.Value?.GetType()}");

        icon.Switch(
            s => Console.WriteLine(s),
            i => Console.WriteLine(i.Url),
            _ => { });

        _markers.Push(marker);

        await marker.AddListener<MouseEvent>("click", async e =>
        {
            //https://github.com/rungwiroon/BlazorGoogleMaps/issues/246
            //var before = marker.EventListeners;
            //await marker.ClearListeners("click");
            //var after = marker.EventListeners;

            var markerLabel = await marker.GetLabel();
            _events.Add("click on " + markerLabel);
            StateHasChanged();

            await e.Stop();
        });
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
        _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
        foreach (var m in _markers)
        {
            var pos = await m.GetPosition();
            await _bounds.Extend(pos);
        }
    }


    private async Task SetAnimation()
    {
        if (!_markers.Any())
        {
            return;
        }
        var lastMarker = _markers.Peek();
        await lastMarker.SetAnimation(Animation.Bounce);
        var position = await lastMarker.GetPosition();
        _events.Add($"SetAnimation {position.Lat},{position.Lng} Animation.Bounce");
    }
    private async Task GetAnimation()
    {
        if (!_markers.Any())
        {
            return;
        }
        var lastMarker = _markers.Peek();
        var animation = await lastMarker.GetAnimation();
        var position = await lastMarker.GetPosition();
        _events.Add($"GetAnimation {position.Lat},{position.Lng} {animation?.ToString()}");
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