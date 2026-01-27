using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Ui.Shared.Pages;

public partial class MapMarker
{
    private GoogleMap _map1;
    private MapOptions _mapOptions;

    private readonly Stack<AdvancedMarkerElement> _markers = new Stack<AdvancedMarkerElement>();
    private readonly List<String> _events = new List<String>();
    private readonly Dictionary<Guid, string> _markerLabels = new Dictionary<Guid, string>();

    private LatLngBounds _bounds;
    private MarkerClustering? _markerClustering;
    public int ZIndex { get; set; }

    [Inject]
    public IJSRuntime JsObjectRef { get; set; }

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral(13.505892, 100.8162),
            MapTypeId = MapTypeId.Roadmap
        };
    }

    private async Task AfterMapRender()
    {
        _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
    }

    private async Task ClearClustering()
    {
        if (_markerClustering != null)
        {
            await _markerClustering.ClearMarkers();
        }
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
            _markerClustering = await MarkerClustering.CreateAsync(_map1.JsRuntime, _map1.InteropObject, new List<AdvancedMarkerElement>(), new MarkerClustererOptions()
            {
                // RendererObjectName = "customRendererLib.interpolatedRenderer"
            });
            await _markerClustering.AddListener("clusteringend", async () => { await SetMarkerListeners(); });
        }
        await _markerClustering.AddMarkers(markers);

        var boundsLiteral = new LatLngBoundsLiteral(coordinates.First());
        foreach (var literal in coordinates)
        {
            LatLngBoundsLiteral.CreateOrExtend(ref boundsLiteral, literal);
        }

        await _map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, GoogleMapsComponents.Maps.Coordinates.Padding>.FromT0(1));

    }

    private List<string>? _listeningLoneMarkerKeys;
    private async Task SetMarkerListeners()
    {
        if (_listeningLoneMarkerKeys == null)
        {
            _listeningLoneMarkerKeys = new List<string>();
        }

        //use GetMappedValue<T> to map and extract the array of guid keys of unclustered markers
        JsObjectRef jsRef = new GoogleMapsComponents.JsObjectRef(JsObjectRef, _markerClustering.Guid);
        var guidStrings = (await jsRef.GetMappedValue<List<string>>("clusters", "marker", "guidString"))
            .Where((x) => { return x != null; });

        if (!guidStrings.Any())
        {
            return;
        }

        // Among markers not in clusters, find those which don't yet have a listener
        AdvancedMarkerElementList deafLoneMarkersList = await AdvancedMarkerElementList.CreateAsync(JsObjectRef, new Dictionary<string, AdvancedMarkerElementOptions>());
        foreach (var key in guidStrings)
        {
            var markr = _markers.First(x => key == x.Guid.ToString());
            if (_listeningLoneMarkerKeys.Contains(key))
            {
                continue;
            }

            deafLoneMarkersList.BaseListableEntities.Add(key, markr);
            _listeningLoneMarkerKeys.Add(key);
        }

        if (!deafLoneMarkersList.BaseListableEntities.Any())
        {
            return;
        }

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
                Content = $"<div class='map-marker-label'>{labelText}</div>"
            });

            result.Add(marker);
        }


        return result;
    }

    private async Task AddMarkerStyled()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();
        ZIndex++;
        var labelText = $"Test {_markers.Count()}";
        var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            ZIndex = ZIndex,
            Title = labelText,
            GmpClickable = true,
            Content = $"<div class='map-marker-label'>{labelText}</div>"
        });

        _markers.Push(marker);
        _markerLabels[marker.Guid] = labelText;

        return;
    }
    private async Task AddMarker()
    {
        var mapCenter = await _map1.InteropObject.GetCenter();
        ZIndex++;
        var labelText = $"Test {_markers.Count()}";
        var marker = await AdvancedMarkerElement.CreateAsync(_map1.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = mapCenter,
            Map = _map1.InteropObject,
            ZIndex = ZIndex,
            Title = labelText,
            GmpClickable = true,
            Content = new PinElement
            {
                Glyph = labelText
            }
        });

        _markers.Push(marker);
        _markerLabels[marker.Guid] = labelText;

        //return;
        await _bounds.Extend(mapCenter);

        await marker.AddListener<MouseEvent>("click", async e =>
        {
            await e.Stop();
            var markerLabel = _markerLabels.TryGetValue(marker.Guid, out var storedLabel)
                ? storedLabel
                : "marker";
            _events.Add("click on " + markerLabel);
            StateHasChanged();


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
        _markerLabels.Remove(lastMarker.Guid);
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
            _events.Add($"Recenter {pos.Lat},{pos.Lng}");
            await _bounds.Extend(new LatLngLiteral(pos.Lat, pos.Lng));
        }
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

    public void Dispose()
    {
        // Just to show that _bounds can be removed, but has be done manually since it doesn't relate to the map
        _map1.Dispose();
    }
}
