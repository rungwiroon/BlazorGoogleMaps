using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace ServerSideDemo.Pages
{
    public partial class MapRoutes
    {
        private GoogleMap map1;
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
            await dirRend.SetDirections(null);
            await dirRend.SetMap(null);
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
            //Adding a waypoint
            var waypoints = new List<DirectionsWaypoint>();
            waypoints.Add(new DirectionsWaypoint() { Location = "Bethlehem, PA", Stopover = false });

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
            _directionsResult = await dirRend.Route(dr);
            foreach (var route in _directionsResult.Routes.SelectMany(x => x.Legs))
            {
                _durationTotalString += route.DurationInTraffic?.Text;
                _distanceTotalString += route.Distance.Text;
            }
        }
    }
}
