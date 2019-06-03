using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// An overlay that looks like a bubble and is often connected to a marker.
    /// </summary>
    public class InfoWindow : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        /// <summary>
        /// Creates an info window with the given options. 
        /// An InfoWindow can be placed on a map at a particular position or above a marker, depending on what is specified in the options. 
        /// Unless auto-pan is disabled, an InfoWindow will pan the map to make itself visible when it is opened. 
        /// After constructing an InfoWindow, you must call open to display it on the map. 
        /// The user can click the close button on the InfoWindow to remove it from the map, or the developer can call close() for the same effect.
        /// </summary>
        /// <param name="opts"></param>
        public async static Task<InfoWindow> CreateAsync(IJSRuntime jsRuntime, InfoWindowOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.InfoWindow", opts);

            var obj = new InfoWindow(jsObjectRef, opts);

            return obj;
        }

        /// <summary>
        /// Creates an info window with the given options. 
        /// An InfoWindow can be placed on a map at a particular position or above a marker, depending on what is specified in the options. 
        /// Unless auto-pan is disabled, an InfoWindow will pan the map to make itself visible when it is opened. 
        /// After constructing an InfoWindow, you must call open to display it on the map. 
        /// The user can click the close button on the InfoWindow to remove it from the map, or the developer can call close() for the same effect.
        /// </summary>
        /// <param name="opts"></param>
        private InfoWindow(JsObjectRef jsObjectRef, InfoWindowOptions opts)
        {
            _jsObjectRef = jsObjectRef;
        }

        public void Dispose()
        {
            _jsObjectRef.Dispose();
        }

        /// <summary>
        /// Closes this InfoWindow by removing it from the DOM structure.
        /// </summary>
        public Task Close()
        {
            return _jsObjectRef.InvokeAsync<bool>("close");
        }

        public Task<string> GetContent()
        {
            return _jsObjectRef.InvokeAsync<string>("getContent");
        }

        public Task<LatLngLiteral> GetPosition()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getPosition");
        }

        public Task<int> GetZIndex()
        {
            return _jsObjectRef.InvokeAsync<int>("getZIndex");
        }

        /// <summary>
        /// Opens this InfoWindow on the given map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="anchor"></param>
        public Task Open(MapComponent map, object anchor = null)
        {
            return _jsObjectRef.InvokeAsync<bool>(map.DivId);
        }

        public Task SetContent(string content)
        {
            return _jsObjectRef.InvokeAsync<object>(content);
        }

        public Task SetPosition(LatLngLiteral position)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setPosition",
                position);
        }

        public Task SetZIndex(int zIndex)
        {
            return _jsObjectRef.InvokeAsync<object>(
                "setZIndex",
                zIndex);
        }
    }
}
