using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace Demo.Ui.Shared.Pages;

public partial class MapRoutes : IAsyncDisposable
{
    private GoogleMap? _map1;
    private MapOptions? _mapOptions;
    private DirectionsRenderer _dirRend;
    private string? _durationTotalString;
    private string? _distanceTotalString;
    private DirectionsResult? _directionsResult;

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
        await _dirRend.SetMap(null);

        _durationTotalString = null;
        _distanceTotalString = null;
    }

    private async Task OnAfterInitAsync()
    {
        //Create instance of DirectionRenderer
        _dirRend = await DirectionsRenderer.CreateAsync(_map1.JsRuntime, new DirectionsRendererOptions()
        {
            Map = _map1.InteropObject,
            MarkerOptions = new MarkerOptions()
            {
                Icon = new Symbol()
                {
                    Path = SymbolPath.BACKWARD_CLOSED_ARROW,
                    Scale = 4,
                    StrokeWeight = 2,
                    FillColor = "white",
                    FillOpacity = 1
                }
            }
        });
    }

    private async Task AddDirections()
    {
        _durationTotalString = null;
        _distanceTotalString = null;
        if (await _dirRend.GetMap() is null)
        {
            await _dirRend.SetMap(_map1!.InteropObject);
        }

        //Adding a waypoint
        var waypoints = new List<DirectionsWaypoint>();
        waypoints.Add(new DirectionsWaypoint() { Location = "Bethlehem, PA", Stopover = true });

        //Direction Request
        var dr = new DirectionsRequest();
        dr.Origin = "Allentown, PA";
        dr.Destination = "Bronx, NY";
        dr.Waypoints = waypoints;
        dr.TravelMode = TravelMode.Driving;
        dr.DrivingOptions = new DrivingOptions()
        {
            DepartureTime = DateTime.Now.AddHours(1)
        };

        //Calculate Route
        _directionsResult = await _dirRend.Route(dr, new DirectionsRequestOptions()
        {
            StripLegsStepsLatLngs = false,
            StripOverviewPath = false,
            StripOverviewPolyline = false,
            StripLegsStepsPath = false,
            StripLegsSteps = false
        });

        if (_directionsResult is null)
        {
            Console.WriteLine("No directions result. Field _directionsResult");
            return;
        }
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

    public async ValueTask DisposeAsync()
    {
        if (_dirRend is not null)
        {
            await _dirRend.SetMap(null);
            await _dirRend.DisposeAsync();
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