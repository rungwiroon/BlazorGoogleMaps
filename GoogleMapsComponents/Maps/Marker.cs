using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using OneOf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Marker : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        public async static Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Marker", opts);
            var obj = new Marker(jsObjectRef);

            return obj;
        }

        private Marker(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public void Dispose()
        {
            _jsObjectRef.Dispose();
        }

        public async Task<Animation> GetAnimation()
        {
            var animation = await _jsObjectRef.InvokeAsync<string>(
                "getAnimation");

            return Helper.ToEnum<Animation>(animation);
        }

        public Task<bool> GetClickable()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getClickable");
        }

        public Task<string> GetCursor()
        {
            return _jsObjectRef.InvokeAsync<string>(
                "getCursor");
        }

        public Task<bool> GetDraggable()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getDraggable");
        }

        public async Task<OneOf<string, Icon, Symbol>> GetIcon()
        {
            var result = await _jsObjectRef.InvokeAsync<string, Icon, Symbol>(
                "getIcon");

            return result;
        }

        public Task<string> GetLabel()
        {
            return _jsObjectRef.InvokeAsync<string>("getLabel");
        }

        public Task<Map> GetMap()
        {
            return _jsObjectRef.InvokeAsync<Map>(
                "getMap");
        }

        public Task<LatLngLiteral> GetPosition()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>(
                "getPosition");
        }

        public Task<MarkerShape> GetShape()
        {
            return _jsObjectRef.InvokeAsync<MarkerShape>(
                "getShape");
        }

        public Task<string> GetTitle()
        {
            return _jsObjectRef.InvokeAsync<string>(
                "getTitle");
        }

        public Task<bool> GetVisible()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getVisible");
        }

        public Task<int> GetZIndex()
        {
            return _jsObjectRef.InvokeAsync<int>(
                "getZIndex");
        }

        /// <summary>
        /// Start an animation. 
        /// Any ongoing animation will be cancelled. 
        /// Currently supported animations are: BOUNCE, DROP. 
        /// Passing in null will cause any animation to stop.
        /// </summary>
        /// <param name="animation"></param>
        public Task SetAnimation(Animation animation)
        {
            return _jsObjectRef.InvokeAsync(
                "setAnimation",
                animation);
        }

        public Task SetClickable(bool flag)
        {
            return _jsObjectRef.InvokeAsync(
                "setClickable",
                flag);
        }

        public Task SetCursor(string cursor)
        {
            return _jsObjectRef.InvokeAsync(
                "setCursor",
                cursor);
        }

        public Task SetDraggable(bool flag)
        {
            return _jsObjectRef.InvokeAsync(
                "setDraggable",
                flag);
        }

        public Task SetIcon(string icon)
        {
            return _jsObjectRef.InvokeAsync(
                "setIcon",
                icon);
        }

        public Task SetIcon(Icon icon)
        {
            return _jsObjectRef.InvokeAsync(
                "setIcon",
                icon);
        }

        public Task SetLabel(Symbol label)
        {
            return _jsObjectRef.InvokeAsync(
                "setLabel",
                label);
        }

        /// <summary>
        /// Renders the marker on the specified map or panorama. 
        /// If map is set to null, the marker will be removed.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync(
                   "setMap",
                   map);

            //_map = map;
        }

        public Task SetOpacity(float opacity)
        {
            return _jsObjectRef.InvokeAsync(
                "setOpacity",
                opacity);
        }

        public Task SetOptions(MarkerOptions options)
        {
            return _jsObjectRef.InvokeAsync(
                "setOptions",
                options);
        }

        public Task SetPosition(LatLngLiteral latLng)
        {
            return _jsObjectRef.InvokeAsync(
                "setPosition",
                latLng);
        }

        public Task SetShape(MarkerShape shape)
        {
            return _jsObjectRef.InvokeAsync(
                "setShape",
                shape);
        }

        public Task SetTitle(string title)
        {
            return _jsObjectRef.InvokeAsync(
                "setTitle",
                title);
        }

        public Task SetVisible(bool visible)
        {
            return _jsObjectRef.InvokeAsync(
                "setVisible",
                visible);
        }

        public Task SetZIndex(int zIndex)
        {
            return _jsObjectRef.InvokeAsync(
                "setZIndex",
                zIndex);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);

            return new MapEventListener(listenerRef);
        }
    }
}
