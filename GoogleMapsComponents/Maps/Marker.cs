using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Marker : JsObjectRef
    {
        private MapComponent _map;

        public Marker(MarkerOptions opt = null)
        {
            if (opt?.Map != null)
                _map = opt.Map;

            Helper.MyInvokeAsync<bool>(
                "googleMapMarkerJsFunctions.init",
                _guid,
                opt);
        }

        public override void Dispose()
        {
            JSRuntime.Current.InvokeAsync<bool>(
                "googleMapMarkerJsFunctions.dispose",
                _guid);
        }

        public async Task<Animation> GetAnimation()
        {
            var animation = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getAnimation");

            return Helper.ToEnum<Animation>(animation);
        }

        public async Task<bool> GetClickable()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getClickable");

            return result;
        }

        public async Task<string> GetCursor()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getCursor");

            return result;
        }

        public async Task<bool> GetDraggable()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getDraggable");

            return result;
        }

        public async Task<object> GetIcon()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getIcon");

            return result;
        }

        public async Task<MarkerLabel> GetLabel()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<MarkerLabel>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
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
                _guid.ToString(),
                "getPosition");

            return result;
        }

        public async Task<MarkerShape> GetShape()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<MarkerShape>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getShape");

            return result;
        }

        public async Task<string> GetTitle()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getTitle");

            return result;
        }

        public async Task<bool> GetVisible()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "getVisible");

            return result;
        }

        public async Task<int> GetZIndex()
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
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
                _guid.ToString(),
                "setAnimation",
                animation);
        }

        public async Task SetClickable(bool flag)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setClickable",
                flag);
        }

        public async Task SetCursor(string cursor)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setCursor",
                cursor);
        }

        public async Task SetDraggable(bool flag)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setDraggable",
                flag);
        }

        public async Task SetIcon(string icon)
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setIcon",
                icon);
        }

        public async Task SetIcon(Icon icon)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setIcon",
                icon);
        }

        public async Task SetLabel(Symbol label)
        {
            var result = await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
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
                   _guid,
                   map?.DivId);

            _map = map;
        }

        public async Task SetOpacity(float opacity)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setOpacity",
                opacity);
        }

        public async Task SetOptions(MarkerOptions options)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setOptions",
                options);
        }

        public async Task SetPosition(LatLngLiteral latLng)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setPosition",
                latLng);
        }

        public async Task SetShape(MarkerShape shape)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setShape",
                shape);
        }

        public async Task SetTiltle(string title)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setTiltle",
                title);
        }

        public async Task SetVisible(bool visible)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setVisible",
                visible);
        }

        public async Task SetZIndex(int zIndex)
        {
            await Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapMarkerJsFunctions.invoke",
                _guid.ToString(),
                "setZIndex",
                zIndex);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action<MapEventArgs> handler)
        {
            var guid = await MapEventJsInterop.SubscribeMarkerEvent(_guid.ToString(), eventName, (dict) =>
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
