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

        private readonly JsObjectRef _jsObjectRef;
        //private readonly MapEventJsInterop _jsEventInterop;

        public async static Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Marker", opts);

            var obj = new Marker(jsObjectRef, opts);

            return obj;
        }

        private Marker(
            JsObjectRef jsObjectRef,
            //MapEventJsInterop jsEventInterop,
            MarkerOptions opt = null)
        {
            _jsObjectRef = jsObjectRef;

            if (opt?.Map != null)
                _map = opt.Map;

            //_jsEventInterop = jsEventInterop;
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

        public Task<object> GetIcon()
        {
            return _jsObjectRef.InvokeAsync<object>(
                "getIcon");
        }

        public Task<MarkerLabel> GetLabel()
        {
            return _jsObjectRef.InvokeAsync<MarkerLabel>(
                "getLabel");
        }

        public MapComponent GetMap()
        {
            return _map;
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
            return _jsObjectRef.InvokeAsync<object>(
                "setAnimation",
                animation);
        }

        public Task SetClickable(bool flag)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setClickable",
                flag);
        }

        public Task SetCursor(string cursor)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setCursor",
                cursor);
        }

        public Task SetDraggable(bool flag)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setDraggable",
                flag);
        }

        public Task SetIcon(string icon)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setIcon",
                icon);
        }

        public Task SetIcon(Icon icon)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setIcon",
                icon);
        }

        public Task SetLabel(Symbol label)
        {
            return _jsObjectRef.InvokeAsync<object>(
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
            await _jsObjectRef.InvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setMap",
                   map?.DivId);

            _map = map;
        }

        public Task SetOpacity(float opacity)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setOpacity",
                opacity);
        }

        public Task SetOptions(MarkerOptions options)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setOptions",
                options);
        }

        public Task SetPosition(LatLngLiteral latLng)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setPosition",
                latLng);
        }

        public Task SetShape(MarkerShape shape)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setShape",
                shape);
        }

        public Task SetTiltle(string title)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setTiltle",
                title);
        }

        public Task SetVisible(bool visible)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setVisible",
                visible);
        }

        public Task SetZIndex(int zIndex)
        {
            return _jsObjectRef.InvokeAsync<object>(
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

            //var guid = await _jsEventInterop.SubscribeMarkerEvent(_guid.ToString(), eventName, (dict) =>
            //{
            //    //if(dict != null)
            //    //{
            //    //    Debug.WriteLine($"{eventName} triggered.");
            //    //    foreach (var val in dict)
            //    //    {
            //    //        Debug.WriteLine(val);
            //    //    }
            //    //}

            //    switch(eventName)
            //    {
            //        //case "click":
            //        //    handler(new MouseEventArgs((string)dict["id"])
            //        //    {

            //        //    });
            //        //    break;

            //        default:
            //            handler(MapEventArgs.Empty);
            //            break;
            //    }
            //});

            //return new MapEventListener(_jsRuntime, _jsEventInterop, guid);
        }
    }
}
