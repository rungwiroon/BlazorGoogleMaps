using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace ServerSideDemo.Pages
{
    public partial class MapRoutes : IAsyncDisposable
    {
        private GoogleMap? map1;
        private MapOptions mapOptions;
        private DirectionsRenderer dirRend;
        private string _durationTotalString;
        private string _distanceTotalString;
        private DirectionsResult _directionsResult;

        protected override void OnInitialized()
        {
            mapOptions = new MapOptions()
            {
                Zoom = 13,
                Center = new LatLngLiteral()
                {
                    Lat = 40.603629,
                    Lng = -75.472518
                },
                MapTypeId = MapTypeId.Roadmap
            };
        }

        private async Task RemoveRoute()
        {
            await dirRend.SetMap(null);

            _durationTotalString = null;
            _distanceTotalString = null;
        }

        private async Task OnAfterInitAsync()
        {
            //Create instance of DirectionRenderer
            dirRend = await DirectionsRenderer.CreateAsync(map1.JsRuntime, new DirectionsRendererOptions()
            {
                Map = map1.InteropObject
            });
        }

        private async Task AddDirections()
        {
            _durationTotalString = null;
            _distanceTotalString = null;
            if (await dirRend.GetMap() is null) await dirRend.SetMap(map1!.InteropObject);

            //Adding a waypoint
            var waypoints = new List<DirectionsWaypoint>();
            waypoints.Add(new DirectionsWaypoint() { Location = "Bethlehem, PA", Stopover = true });

            //Direction Request
            DirectionsRequest dr = new DirectionsRequest();
            dr.Origin = "Allentown, PA";
            dr.Destination = "Bronx, NY";
            dr.Waypoints = waypoints;
            dr.TravelMode = TravelMode.Driving;
            dr.DrivingOptions = new DrivingOptions()
            {
                DepartureTime = DateTime.Now.AddHours(1)
            };

            //Calculate Route
            _directionsResult = await dirRend.Route(dr, new DirectionsRequestOptions()
            {
                StripLegsStepsLatLngs = false,
                StripOverviewPath = false,
                StripOverviewPolyline = false,
                StripLegsStepsPath = false,
                StripLegsSteps = false
            });


            var routes = _directionsResult.Routes.SelectMany(x => x.Legs).ToList();

            foreach (var route in routes)
            {
                _durationTotalString += route.DurationInTraffic?.Text;
                _distanceTotalString += route.Distance.Text;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (dirRend is not null)
            {
                await dirRend.SetMap(null);
                dirRend.Dispose();
            }
        }
    }
}
