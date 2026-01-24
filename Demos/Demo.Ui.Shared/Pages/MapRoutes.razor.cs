using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;

namespace Demo.Ui.Shared.Pages;

public partial class MapRoutes : IAsyncDisposable
{
    private GoogleMap? _map1;
    private MapOptions? _mapOptions;
    private DirectionsRenderer _dirRend;
    private string? _durationTotalString;
    private string? _distanceTotalString;
    private DirectionsResult? _directionsResult;
    private readonly Stack<AdvancedMarkerElement> _routeMarkers = new();
    private string? _lastError;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral(13.505892, 100.8162),
            MapTypeId = MapTypeId.Roadmap
        };
    }

    private async Task RemoveRoute()
    {
        _lastError = null;
        try
        {
            if (_dirRend is not null)
            {
                await _dirRend.SetMap(null);
            }
            await ClearRouteMarkersAsync();

            _durationTotalString = null;
            _distanceTotalString = null;
        }
        catch (Exception ex)
        {
            _lastError = ex.Message;
        }
    }

    private async Task OnAfterInitAsync()
    {
        //Create instance of DirectionRenderer
        _dirRend = await DirectionsRenderer.CreateAsync(_map1.JsRuntime, new DirectionsRendererOptions()
        {
            Map = _map1.InteropObject,
            SuppressMarkers = true
        });
    }

    private async Task AddDirections()
    {
        _durationTotalString = null;
        _distanceTotalString = null;
        _lastError = null;

        if (_map1 is null)
        {
            _lastError = "Map is not initialized yet.";
            return;
        }

        if (_dirRend is null)
        {
            _lastError = "Directions renderer is not initialized yet.";
            return;
        }

        try
        {
            if (await _dirRend.GetMap() is null)
            {
                await _dirRend.SetMap(_map1.InteropObject);
            }

            //Adding a waypoint
            var waypoints = new List<DirectionsWaypoint>
            {
                new() { Location = "Bethlehem, PA", Stopover = true }
            };

            //Direction Request
            var dr = new DirectionsRequest
            {
                Origin = "Allentown, PA",
                Destination = "Bronx, NY",
                Waypoints = waypoints,
                TravelMode = TravelMode.Walking,
                DrivingOptions = new DrivingOptions
                {
                    DepartureTime = DateTime.Now.AddHours(1)
                }
            };

            //Calculate Route
            _directionsResult = await _dirRend.Route(dr, new DirectionsRequestOptions()
            {
                StripLegsStepsLatLngs = true,
                StripOverviewPath = true,
                StripOverviewPolyline = true,
                StripLegsStepsPath = true,
                StripLegsSteps = true,
            });

            if (_directionsResult is null)
            {
                _lastError = "No directions result.";
                return;
            }

            await RenderRouteMarkersAsync();
            var routes = _directionsResult.Routes.SelectMany(x => x.Legs).ToList();

            foreach (var route in routes)
            {
                _durationTotalString += route.DurationInTraffic?.Text;
                _distanceTotalString += route.Distance?.Text;

                if (route.Steps is null)
                {
                    Console.WriteLine("No steps in route");
                    continue;
                }

                foreach (var step in route.Steps)
                {
                    Console.WriteLine($"Step: {step.Instructions}, Distance: {step.Distance?.Text}, Duration: {step.Duration?.Text}");
                    Console.WriteLine(step.StartLocation);
                    Console.WriteLine(step.TravelMode);
                }
            }
        }
        catch (Exception ex)
        {
            _lastError = ex.ToString();
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_dirRend is not null)
            {
                await _dirRend.SetMap(null);
                await _dirRend.DisposeAsync();
            }

            await ClearRouteMarkersAsync();
        }
        catch (JSDisconnectedException)
        {
            // Ignore: circuit is already disconnected.
        }
        catch (TaskCanceledException)
        {
            // Ignore: circuit is shutting down.
        }
        catch (ObjectDisposedException)
        {
            // Ignore: circuit is disposed.
        }
    }

    private async Task RenderRouteMarkersAsync()
    {
        await ClearRouteMarkersAsync();

        var firstLeg = _directionsResult?.Routes?.FirstOrDefault()?.Legs?.FirstOrDefault();
        if (firstLeg == null)
        {
            return;
        }

        await AddRouteMarkerAsync(firstLeg.StartLocation, "Start");
        await AddRouteMarkerAsync(firstLeg.EndLocation, "End");
    }

    private async Task AddRouteMarkerAsync(LatLngLiteral position, string title)
    {
        var marker = await AdvancedMarkerElement.CreateAsync(_map1!.JsRuntime, new AdvancedMarkerElementOptions()
        {
            Position = position,
            Map = _map1.InteropObject,
            Title = title,
            GmpClickable = true,
            Content = new PinElement
            {
                Glyph = title
            }
        });

        _routeMarkers.Push(marker);
    }

    private async Task ClearRouteMarkersAsync()
    {
        while (_routeMarkers.Count > 0)
        {
            var marker = _routeMarkers.Pop();
            try
            {
                await marker.SetMap(null);
                await marker.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                break;
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
        }
    }

    private async Task RunDirectionsService()
    {
        var service = await DirectionsService.CreateAsync(_map1!.JsRuntime);

        DirectionsRequest request = new()
        {
            Origin = "Orlando, FL",
            Destination = "Miami, FL"
        };

        var result = await service.Route(request);
        if (result == null)
        {
            Console.WriteLine("No result");
        }
        else
        {
            Console.WriteLine(result.Routes?.FirstOrDefault()?.Summary ?? "No routes?");
        }
    }
}
