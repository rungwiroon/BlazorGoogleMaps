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
    public class InfoWindow : JsObjectRef
    {
        /// <summary>
        /// Creates an info window with the given options. 
        /// An InfoWindow can be placed on a map at a particular position or above a marker, depending on what is specified in the options. 
        /// Unless auto-pan is disabled, an InfoWindow will pan the map to make itself visible when it is opened. 
        /// After constructing an InfoWindow, you must call open to display it on the map. 
        /// The user can click the close button on the InfoWindow to remove it from the map, or the developer can call close() for the same effect.
        /// </summary>
        /// <param name="opts"></param>
        public InfoWindow(IJSRuntime jsRuntime, InfoWindowOptions opts)
            : base(jsRuntime, "google.maps.InfoWindow", opts)
        {
            _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                "googleMapInfoWindowJsFunctions.init",
                Guid.ToString(),
                opts);
        }

        public override void Dispose()
        {
            _jsRuntime.InvokeAsync<bool>(
                "googleMapInfoWindowJsFunctions.dispose",
                Guid.ToString());
        }

        /// <summary>
        /// Closes this InfoWindow by removing it from the DOM structure.
        /// </summary>
        public Task Close()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<bool>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "close");
        }

        public Task<string> GetContent()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "getContent");
        }

        public Task<LatLngLiteral> GetPosition()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<LatLngLiteral>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "getPosition");
        }

        public Task<int> GetZIndex()
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "getZIndex");
        }

        /// <summary>
        /// Opens this InfoWindow on the given map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="anchor"></param>
        public Task Open(MapComponent map, object anchor = null)
        {
            return _jsRuntime.InvokeWithDefinedGuidAsync<bool>(
                "googleMapInfoWindowJsFunctions.open",
                Guid.ToString(),
                map.DivId);
        }

        public Task SetContent(string content)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "setContent",
                content);
        }

        public Task SetPosition(LatLngLiteral position)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "setPosition",
                position);
        }

        public Task SetZIndex(int zIndex)
        {
            return _jsRuntime.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapInfoWindowJsFunctions.invoke",
                Guid.ToString(),
                "setZIndex",
                zIndex);
        }
    }
}
