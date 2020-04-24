using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Coordinates;
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

        private async Task AddMarker()
        {
            var mapCenter = await map1.InteropObject.GetCenter();

            var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
            {
                Position = mapCenter,
                Map = map1.InteropObject,
                Label = $"Test {markers.Count}",
                Icon = new Icon()
                {
                    Url = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
                }
                //Icon = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
            });

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
