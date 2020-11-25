using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using ServerSideDemo.Shared;

namespace ServerSideDemo.Pages
{
    public partial class MapListOfCircles
    {
        static readonly List<LatLngLiteral> Coordinates = GetLotsOfCoordinates();

        private static List<LatLngLiteral> GetLotsOfCoordinates()
        {
            return new[]{(13.509159517380125, 100.81979269125533),
                        (13.509201245364215, 100.81974977591109),
                        (13.509201245364215, 100.81970686056685),
                        (13.509242973341033, 100.81970686056685),
                        (13.509242973341033, 100.81966394522262),
                        (13.509284701310518, 100.81962102987838),
                        (13.509284701310518, 100.81957811453414),
                        (13.509284701310518, 100.8195351991899),
                        (13.509284701310518, 100.81940645315719),
                        (13.509284701310518, 100.81932062246871),
                        (13.509284701310518, 100.81923479178023),
                        (13.509326429272715, 100.81914896109176),
                        (13.509326429272715, 100.81902021505904),
                        (13.509326429272715, 100.81893438437056),
                        (13.509326429272715, 100.81880563833785),
                        (13.509326429272715, 100.81867689230513),
                        (13.509368157227609, 100.81854814627242),
                        (13.509409885175202, 100.8184194002397),
                        (13.509451613115491, 100.81820482351851),
                        (13.509451613115491, 100.8180760774858),
                        (13.509493341048492, 100.81794733145308),
                        (13.50953506897419, 100.81786150076461),
                        (13.509576796892558, 100.81777567007613),
                        (13.509576796892558, 100.81773275473189),
                        (13.509618524803649, 100.81764692404342),
                        (13.509618524803649, 100.81756109335494),
                        (13.509618524803649, 100.81747526266646),
                        (13.50966025270744, 100.81734651663375),
                        (13.50966025270744, 100.81721777060103),
                        (13.50974370849311, 100.8169602785356),
                        (13.50974370849311, 100.81683153250289),
                        (13.50974370849311, 100.81670278647017),
                        (13.509785436374992, 100.81657404043746),
                        (13.509785436374992, 100.81644529440474),
                        (13.509785436374992, 100.81631654837203),
                        (13.509785436374992, 100.81614488699508),
                        (13.509785436374992, 100.81597322561812),
                        (13.509785436374992, 100.81584447958541),
                        (13.509785436374992, 100.81575864889693),
                        (13.509785436374992, 100.81562990286422),
                        (13.509785436374992, 100.81554407217574),
                        (13.509785436374992, 100.81541532614303),
                        (13.509785436374992, 100.81528658011031),
                        (13.509785436374992, 100.81524366476607),
                        (13.50982716424957, 100.81520074942183),
                        (13.50982716424957, 100.8151578340776),
                        (13.50982716424957, 100.81511491873336),
                        (13.50982716424957, 100.81498617270064),
                        (13.509910619976834, 100.81468576529097),
                        (13.509952347829493, 100.81434244253707),
                        (13.509994075674875, 100.81425661184859),
                        (13.509994075674875, 100.81412786581588),
                        (13.509994075674875, 100.81408495047164),
                        (13.510035803512954, 100.81395620443892),
                        (13.510035803512954, 100.81387037375045),
                        (13.510077531343716, 100.81378454306197),
                        (13.510077531343716, 100.81374162771773),
                        (13.510077531343716, 100.8136987123735),
                        (13.510077531343716, 100.81356996634078),
                        (13.510119259167192, 100.8134841356523),
                        (13.510119259167192, 100.81335538961959),
                        (13.510119259167192, 100.81326955893111),
                        (13.510119259167192, 100.81322664358687),
                        (13.510160986983363, 100.81318372824263),
                        (13.510160986983363, 100.81305498220992),
                        (13.510160986983363, 100.81288332083297),
                        (13.510160986983363, 100.81275457480025),
                        (13.510160986983363, 100.81262582876754),
                        (13.510160986983363, 100.81253999807906),
                        (13.510160986983363, 100.81249708273482),
                        (13.510160986983363, 100.81241125204635),
                        (13.510160986983363, 100.81232542135787),
                        (13.510160986983363, 100.81223959066939),
                        (13.510160986983363, 100.81215375998092),
                        (13.510160986983363, 100.81198209860396),
                        (13.510119259167192, 100.81181043722701),
                        (13.510077531343716, 100.81172460653853),
                        (13.510077531343716, 100.81163877585006),
                        (13.509994075674875, 100.81151002981734),
                        (13.509994075674875, 100.81142419912887),
                        (13.509994075674875, 100.81120962240767),
                        (13.509994075674875, 100.8111237917192),
                        (13.509994075674875, 100.81103796103072),
                        (13.509994075674875, 100.810909214998),
                        (13.509994075674875, 100.81073755362105),
                        (13.509952347829493, 100.81039423086715),
                        (13.509868892116849, 100.81017965414595),
                        (13.509868892116849, 100.81013673880172),
                        (13.509868892116849, 100.81005090811324),
                        (13.509868892116849, 100.810007992769),
                        (13.50982716424957, 100.80996507742476),
                        (13.50982716424957, 100.80987924673629),
                        (13.50974370849311, 100.80983633139205),
                        (13.509701980603927, 100.80979341604781),
                        (13.50966025270744, 100.80975050070357),
                        (13.509576796892558, 100.8096646700151),
                        (13.509493341048492, 100.80957883932662),
                        (13.509409885175202, 100.80940717794967),
                        (13.509409885175202, 100.8091695)}
                        .Select(tpl => new LatLngLiteral(tpl.Item2, tpl.Item1)).ToList();
        }

        private Map map;
        private ElementReference ElementRef;

        private MapEventList eventList;
        private readonly List<string> _events = new List<string>();

        public CircleList Circles { get; private set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (map == null)
            {
                var mapOptions = new MapOptions()
                {
                    Zoom = 16,
                    Center = Coordinates[Coordinates.Count / 2],
                    MapTypeId = MapTypeId.Sattellite
                };
                map = await Map.CreateAsync(JsRuntime, ElementRef, mapOptions);
            }
        }

        private async Task AddCircles()
        {
            this._events.Clear();
            if (this.Circles != null)
            {
                await this.Circles.RemoveMultipleAsync(this.Circles.Circles.Keys.ToList());
                this.Circles.Dispose();
                this.Circles = null;
            }

            CircleList circles = await this.DrawCircles();
            //await this.AttachEvents(circles);
            await this.AttachBatchEvents(circles);            

            this.Circles = circles;
            
            this._events.Add($"Added {circles.BaseListableEntities.Count} circles");
            await this.InvokeAsync(StateHasChanged);
        }

        private async Task<CircleList> DrawCircles()
        {
            Dictionary<string, CircleOptions> options = this.GetCirclesOptions();
            return await CircleList.CreateAsync(JsRuntime, options);
        }

        private async Task AttachEvents(CircleList circles)
        {
            foreach (KeyValuePair<string, Circle> pair in circles.BaseListableEntities)
            {
                Circle circle = pair.Value;

                Action clickAction = async () =>
                {
                    var center = await circle.GetCenter();
                    this._events.Add($"{center} Clicked");
                    await this.InvokeAsync(StateHasChanged);
                };

                await circle.AddListener("click", clickAction);
            }
        }

        private async Task AttachBatchEvents(CircleList circles)
        {
            Action<Circle> clickAction = async (circle) =>
            {
                var center = await circle.GetCenter();
                this._events.Add($"{center} Clicked");
                await this.InvokeAsync(StateHasChanged);
            };

            await circles.AddListener("click", clickAction);
        }



        private Dictionary<string, CircleOptions> GetCirclesOptions()
        {
            return Coordinates.Select(c => new CircleOptions()
            {
                Center = c,
                Clickable = true,
                FillColor = "#00FF00",
                Radius = 5,
                FillOpacity = 1,
                Map = this.map,
            })
                .ToDictionary(op => op.Center.ToString(), op => op);

        }


    }
}
