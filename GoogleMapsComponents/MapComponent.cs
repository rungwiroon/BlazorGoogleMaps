using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class MapComponent : BlazorComponent, IDisposable
    {
        public string DivId { get; private set; }

        public async Task InitAsync(string id, MapOptions options)
        {
            DivId = id;

            await MapFunctionJsInterop.Init(id, options);

            MapComponentInstances.Add(id, this);
        }

        public async Task InitAsync(ElementRef element, MapOptions options)
        {
            DivId = Guid.NewGuid().ToString();

            var optionsJson = JsonConvert.SerializeObject(options,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            });

            await Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.init",
                DivId,
                element,
                optionsJson);

            MapComponentInstances.Add(DivId, this);
        }

        public void Dispose()
        {
            ClearListeners();
            MapFunctionJsInterop.Dispose(DivId);
            MapComponentInstances.Remove(DivId);
        }

        /// <summary>
        /// Sets the viewport to contain the given bounds.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public Task FitBounds(LatLngBoundsLiteral bounds)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "fitBounds",
                bounds);
        }

        /// <summary>
        /// Changes the center of the map by the given distance in pixels.
        /// If the distance is less than both the width and height of the map, the transition will be smoothly animated.
        /// Note that the map coordinate system increases from west to east (for x values) and north to south (for y values).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Task PanBy(int x, int y)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "panBy",
                x,
                y);
        }

        /// <summary>
        /// Changes the center of the map to the given LatLng.
        /// If the change is less than both the width and height of the map, the transition will be smoothly animated.
        /// </summary>
        /// <param name="latLng"></param>
        /// <returns></returns>
        public Task PanTo(LatLngLiteral latLng)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "panTo",
                latLng);
        }

        /// <summary>
        /// Pans the map by the minimum amount necessary to contain the given LatLngBounds.
        /// It makes no guarantee where on the map the bounds will be, except that the map will be panned to show as much of the bounds as possible inside {currentMapSizeInPx} - {padding}.
        /// </summary>
        /// <param name="latLngBounds"></param>
        /// <returns></returns>
        public Task PanToBounds(LatLngBoundsLiteral latLngBounds)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "panToBounds",
                latLngBounds);
        }

        /// <summary>
        /// Returns the lat/lng bounds of the current viewport.
        /// If more than one copy of the world is visible, the bounds range in longitude from -180 to 180 degrees inclusive.
        /// If the map is not yet initialized (i.e. the mapType is still null), or center and zoom have not been set then the result is null.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngBoundsLiteral> GetBounds()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<LatLngBoundsLiteral>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getBounds");
        }

        /// <summary>
        /// Returns the position displayed at the center of the map.
        /// Note that this LatLng object is not wrapped.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetCenter()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<LatLngLiteral>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getCenter");
        }

        public Task SetCenter(LatLngLiteral latLng)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "setCenter",
                latLng);
        }

        /// <summary>
        /// Returns the compass heading of aerial imagery.
        /// The heading value is measured in degrees (clockwise) from cardinal direction North.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetHeading()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getHeading");
        }

        /// <summary>
        /// Sets the compass heading for aerial imagery measured in degrees from cardinal direction North.
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public Task SetHeading(int heading)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "setHeading",
                heading);
        }

        public async Task<MapTypeId> GetMapTypeId()
        {
             var mapTypeIdStr = await Helper.InvokeWithDefinedGuidAndMethodAsync<string>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getMapTypeId");

            return Helper.ToEnum<MapTypeId>(mapTypeIdStr);
        }

        public Task SetMapTypeId(MapTypeId mapTypeId)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "setMapTypeId",
                mapTypeId);
        }

        /// <summary>
        /// Returns the current angle of incidence of the map, in degrees from the viewport plane to the map plane.
        /// The result will be 0 for imagery taken directly overhead or 45 for 45° imagery. 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels.
        /// Note: This method does not return the value set by setTilt.
        /// See setTilt for details.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetTilt()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getTilt");
        }

        /// <summary>
        /// Controls the automatic switching behavior for the angle of incidence of the map.
        /// The only allowed values are 0 and 45.
        /// setTilt(0) causes the map to always use a 0° overhead view regardless of the zoom level and viewport.
        /// setTilt(45) causes the tilt angle to automatically switch to 45 whenever 45° imagery is available for the current zoom level and viewport, and switch back to 0 whenever 45° imagery is not available (this is the default behavior).
        /// 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels. Note: getTilt returns the current tilt angle, not the value set by setTilt.
        /// Because getTilt and setTilt refer to different things, do not bind() the tilt property; doing so may yield unpredictable effects.
        /// </summary>
        /// <param name="tilt"></param>
        /// <returns></returns>
        public Task SetTilt(int tilt)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "setTilt",
                tilt);
        }

        public Task<int> GetZoom()
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<int>(
                "googleMapJsFunctions.invoke",
                DivId,
                "getZoom");
        }

        public Task SetZoom(int zoom)
        {
            return Helper.InvokeWithDefinedGuidAndMethodAsync<object>(
                "googleMapJsFunctions.invoke",
                DivId,
                "setZoom",
                zoom);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action<MapEventArgs> handler)
        {
            var guid = await MapEventJsInterop.SubscribeMapEvent(DivId, eventName, (jObject) =>
            {
                if (jObject != null)
                {
                    Debug.WriteLine($"{eventName} triggered.");
                    //foreach (var val in dict)
                    //{
                        Debug.WriteLine(jObject);
                    //}
                }

                switch (eventName)
                {
                    case "click":
                        var e = jObject.ToObject<MouseEventArgs>();
                        //Debug.WriteLine($"Click lat lng : {e.LatLng}");
                        handler(e);
                        break;

                    default:
                        handler(MapEventArgs.Empty);
                        break;
                }
            });

            return new MapEventListener(guid);
        }

        public async Task<MapEventListener> AddListenerOnce(string eventName, Action<MapEventArgs> handler)
        {
            var guid = await MapEventJsInterop.SubscribeMapEventOnce(DivId, eventName, (jObject) =>
            {
                //if (jObject != null)
                //{
                    //Debug.WriteLine($"{eventName} triggered.");
                    //foreach (var val in dict)
                    //{
                    //Debug.WriteLine(jObject);
                    //}
                //}

                switch (eventName)
                {
                    case "click":
                        var e = jObject.ToObject<MouseEventArgs>();
                        //Debug.WriteLine($"Click lat lng : {e.LatLng}");
                        handler(e);
                        break;

                    default:
                        handler(MapEventArgs.Empty);
                        break;
                }
            });

            return new MapEventListener(guid);
        }

        public Task ClearListeners()
        {
           return JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.clearInstanceListeners",
                DivId);
        }

        public Task clearListeners(string eventName)
        {
            return JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.clearListeners",
                DivId,
                eventName);
        }

        public Task RemoveListener(MapEventListener listerner)
        {
            return listerner.Remove();
        }
    }
}
