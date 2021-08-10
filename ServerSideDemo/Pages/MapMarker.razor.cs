using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ServerSideDemo.Shared;

namespace ServerSideDemo.Pages
{
    public partial class MapMarker
    {
        private GoogleMap map1;

        private MapOptions mapOptions;

        private Stack<Marker> markers = new Stack<Marker>();

        private List<String> _events = new List<String>();

        private MapEventList eventList;

        private LatLngBounds bounds;
        private MarkerClustering _markerClustering;
        public int ZIndex { get; set; } = 0;

        [Inject]
        public IJSRuntime JsObjectRef { get; set; }

        protected override void OnInitialized()
        {
            mapOptions = new MapOptions()
            {
                Zoom = 13,
                Center = new LatLngLiteral()
                {
                    Lat = 13.505892,
                    Lng = 100.8162
                },
                MapTypeId = MapTypeId.Roadmap
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                bounds = await LatLngBounds.CreateAsync(map1.JsRuntime);
            }
        }

        private async Task ClearClustering()
        {
            await _markerClustering.ClearMarkers();
        }
        private async Task InvokeClustering()
        {
            var coordinates = new List<LatLngLiteral>()
            {
                new LatLngLiteral(147.154312, -31.56391),
                new LatLngLiteral(150.363181, -33.718234),
                new LatLngLiteral(150.371124, -33.727111),
                new LatLngLiteral(151.209834, -33.848588),
                new LatLngLiteral(151.216968, -33.851702),
                new LatLngLiteral(150.863657, -34.671264),
                new LatLngLiteral(148.662905, -35.304724),
                new LatLngLiteral(175.699196, -36.817685),
                new LatLngLiteral(175.790222, -36.828611),
                new LatLngLiteral(145.116667, -37.75),
                new LatLngLiteral(145.128708, -37.759859),
                new LatLngLiteral(145.133858, -37.765015),
                new LatLngLiteral(145.143299, -37.770104),
                new LatLngLiteral(145.145187, -37.7737),
                new LatLngLiteral(145.137978, -37.774785),
                new LatLngLiteral(144.968119, -37.819616),
                new LatLngLiteral(144.695692, -38.330766),
                new LatLngLiteral(175.053218, -39.927193),
                new LatLngLiteral(174.865694, -41.330162),
                new LatLngLiteral(147.439506, -42.734358),
                new LatLngLiteral(147.501315, -42.734358),
                new LatLngLiteral(147.438, -42.735258),
                new LatLngLiteral(170.463352, -43.999792),
            };

            var markers = await GetMarkers(coordinates, map1.InteropObject);

            _markerClustering = await MarkerClustering.CreateAsync(map1.JsRuntime, map1.InteropObject, markers);
            await _markerClustering.FitMapToMarkers(1);
            //initMap
            //await JsObjectRef.InvokeAsync<object>("initMap", map1.InteropObject.Guid.ToString(), markers);
        }

        private async Task<IEnumerable<Marker>> GetMarkers(IEnumerable<LatLngLiteral> coords, Map map)
        {
            var result = new List<Marker>(coords.Count());
            var index = 1;
            foreach (var latLngLiteral in coords)
            {
                var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
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
            var mapCenter = await map1.InteropObject.GetCenter();
            ZIndex++;

            var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
            {
                Position = mapCenter,
                Map = map1.InteropObject,
                ZIndex = ZIndex,
                Label = new MarkerLabel
                {
                    Text = $"Test {markers.Count()}",
                    FontWeight = "bold",
                    Color = "#5B32FF",
                    FontSize = "24",
                    ClassName = "map-marker-label"
                },
            });

            markers.Push(marker);

            return;
        }
        private async Task AddMarker()
        {
            var mapCenter = await map1.InteropObject.GetCenter();
            ZIndex++;

            var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
            {
                Position = mapCenter,
                Map = map1.InteropObject,
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

            markers.Push(marker);

            return;
            await bounds.Extend(mapCenter);

            var icon = await marker.GetIcon();

            Console.WriteLine($"Get icon result type is : {icon.Value.GetType()}");

            icon.Switch(
                s => Console.WriteLine(s),
                i => Console.WriteLine(i.Url),
                _ => { });

            markers.Push(marker);

            await marker.AddListener<MouseEvent>("click", async e =>
            {
                var markerLabel = await marker.GetLabel();
                _events.Add("click on " + markerLabel);
                StateHasChanged();

                await e.Stop();
            });
        }

        private async Task RemoveMarker()
        {
            if (!markers.Any())
            {
                return;
            }

            var lastMarker = markers.Pop();
            await lastMarker.SetMap(null);
        }

        private async Task Recenter()
        {
            if (!markers.Any())
            {
                return;
            }

            var lastMarker = markers.Peek();
            var center = await map1.InteropObject.GetCenter();
            await lastMarker.SetPosition(center);
        }


        private async Task SetAnimation()
        {
            if (!markers.Any())
            {
                return;
            }
            var lastMarker = markers.Peek();
            await lastMarker.SetAnimation(Animation.Bounce);
            var position = await lastMarker.GetPosition();
            _events.Add($"SetAnimation {position.Lat},{position.Lng} Animation.Bounce");
        }
        private async Task GetAnimation()
        {
            if (!markers.Any())
            {
                return;
            }
            var lastMarker = markers.Peek();
            var animation = await lastMarker.GetAnimation();
            var position = await lastMarker.GetPosition();
            _events.Add($"GetAnimation {position.Lat},{position.Lng} {animation?.ToString()}");
        }
        private async Task FitBounds()
        {
            if (await this.bounds.IsEmpty())
            {
                return;
            }

            var boundsLiteral = await bounds.ToJson();
            await map1.InteropObject.FitBounds(boundsLiteral, OneOf.OneOf<int, Padding>.FromT0(5));
        }
    }
}