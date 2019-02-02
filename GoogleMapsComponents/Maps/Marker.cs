using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Marker : IDisposable
    {
        private MapComponent _map;
        public Guid Guid { get; private set; }

        public Marker(MarkerOptions opt)
        {
            Guid = Guid.NewGuid();

            if (opt?.Map != null)
                _map = opt.Map;

            Helper.MyInvokeAsync<bool>(
                "googleMapMarkerJsFunctions.init",
                Guid,
                opt);
        }

        public void Dispose()
        {
            JSRuntime.Current.InvokeAsync<bool>(
                "googleMapMarkerJsFunctions.dispose",
                Guid);
        }

        public async Task<Animation> GetAnimation()
        {
            var animation = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getAnimation");

            return Helper.ToEnum<Animation>(animation);
        }

        public async Task<bool> GetClickable()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getClickable");

            return result;
        }

        public async Task<string> GetCursor()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getCursor");

            return result;
        }

        public async Task<bool> GetDraggable()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getDraggable");

            return result;
        }

        public async Task<object> GetIcon()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getIcon");

            return result;
        }

        public async Task<MarkerLabel> GetLabel()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<MarkerLabel>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getLabel");

            return result;
        }

        public MapComponent GetMap()
        {
            return _map;
        }

        public async Task<LatLngLiteral> GetPosition()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<LatLngLiteral>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getPosition");

            return result;
        }

        public async Task<MarkerShape> GetShape()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<MarkerShape>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getShape");

            return result;
        }

        public async Task<string> GetTitle()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getTitle");

            return result;
        }

        public async Task<bool> GetVisible()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getVisible");

            return result;
        }

        public async Task<int> GetZIndex()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "getZIndex");

            return result;
        }

        /// <summary>
        /// Start an animation. 
        /// Any ongoing animation will be cancelled. 
        /// Currently supported animations are: BOUNCE, DROP. 
        /// Passing in null will cause any animation to stop.
        /// </summary>
        /// <param name="animation"></param>
        public async Task SetAnimation(Animation animation)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setAnimation",
                animation);
        }

        public async Task SetClickable(bool flag)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setClickable",
                flag);
        }

        public async Task SetCursor(string cursor)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setCursor",
                cursor);
        }

        public async Task SetDraggable(bool flag)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setDraggable",
                flag);
        }

        public async Task SetIcon(string icon)
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setIcon",
                icon);
        }

        public async Task SetIcon(Icon icon)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setIcon",
                icon);
        }

        public async Task SetLabel(Symbol label)
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setLabel",
                label);
        }

        /// <summary>
        /// Renders the marker on the specified map or panorama. 
        /// If map is set to null, the marker will be removed.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMap(MapComponent map)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setMap",
                   Guid,
                   map?.DivId);

            _map = map;
        }

        public async Task SetOpacity(float opacity)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setOpacity",
                opacity);
        }

        public async Task SetOptions(MarkerOptions options)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setOptions",
                options);
        }

        public async Task SetPosition(LatLngLiteral latLng)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setPosition",
                latLng);
        }

        public async Task SetShape(MarkerShape shape)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setShape",
                shape);
        }

        public async Task SetTiltle(string title)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setTiltle",
                title);
        }

        public async Task SetVisible(bool visible)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setVisible",
                visible);
        }

        public async Task SetZIndex(int zIndex)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                Guid.ToString(),
                "setZIndex",
                zIndex);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action<MapEventArgs> handler)
        {
            var guid = await MapEventJsInterop.SubscribeMarkerEvent(Guid.ToString(), eventName, (dict) =>
            {
                //if(dict != null)
                //{
                //    Debug.WriteLine($"{eventName} triggered.");
                //    foreach (var val in dict)
                //    {
                //        Debug.WriteLine(val);
                //    }
                //}

                switch(eventName)
                {
                    //case "click":
                    //    handler(new MouseEventArgs((string)dict["id"])
                    //    {

                    //    });
                    //    break;

                    default:
                        handler(MapEventArgs.Empty);
                        break;
                }
            });

            return new MapEventListener(guid);
        }
    }
}
