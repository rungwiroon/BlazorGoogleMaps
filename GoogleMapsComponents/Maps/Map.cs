using GoogleMapsComponents.Maps.Coordinates;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// google.maps.Map class
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter))]
    public class Map : MVCObject
    {
        public Task<MapData> Data
        {
            get
            {
                return InvokeAsync<IJSObjectReference>("data")
                    .AsTask()
                    .ContinueWith(dataObjectRef => new MapData(dataObjectRef.Result));
            }
        }

        public Task<object> Controls
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static async Task<Map> CreateAsync(
            IJSRuntime jsRuntime,
            ElementReference mapDiv,
            MapOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                "google.maps.Map",
                mapDiv,
                opts);
            
            var map = new Map(jsObjectRef);

            return map;
        }

        internal Map(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        public ValueTask AddControl(ControlPosition position, ElementReference reference)
        {
            //return _jsObjectRef.InvokeVoidAsync("googleMapsObjectManager.addControls", position, reference);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the viewport to contain the given bounds.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public ValueTask FitBounds(LatLngBoundsLiteral bounds, OneOf<int, Padding>? padding = null)
        {
            return InvokeVoidAsync("fitBounds", bounds, padding);
        }

        /// <summary>
        /// Changes the center of the map by the given distance in pixels.
        /// If the distance is less than both the width and height of the map, the transition will be smoothly animated.
        /// Note that the map coordinate system increases from west to east (for x values) and north to south (for y values).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ValueTask PanBy(int x, int y)
        {
            return InvokeVoidAsync("panBy", x, y);
        }

        /// <summary>
        /// Changes the center of the map to the given LatLng.
        /// If the change is less than both the width and height of the map, the transition will be smoothly animated.
        /// </summary>
        /// <param name="latLng"></param>
        /// <returns></returns>
        public ValueTask PanTo(LatLngLiteral latLng)
        {
            return InvokeVoidAsync("panTo", latLng);
        }

        /// <summary>
        /// Pans the map by the minimum amount necessary to contain the given LatLngBounds.
        /// It makes no guarantee where on the map the bounds will be, except that the map will be panned to show as much of the bounds as possible inside {currentMapSizeInPx} - {padding}.
        /// </summary>
        /// <param name="latLngBounds"></param>
        /// <returns></returns>
        public ValueTask PanToBounds(LatLngBoundsLiteral latLngBounds)
        {
            return InvokeVoidAsync("panToBounds", latLngBounds);
        }

        /// <summary>
        /// Returns the lat/lng bounds of the current viewport.
        /// If more than one copy of the world is visible, the bounds range in longitude from -180 to 180 degrees inclusive.
        /// If the map is not yet initialized (i.e. the mapType is still null), or center and zoom have not been set then the result is null.
        /// </summary>
        /// <returns></returns>
        public ValueTask<LatLngBoundsLiteral> GetBounds()
        {
            return InvokeAsync<LatLngBoundsLiteral>("getBounds");
        }

        /// <summary>
        /// Returns the position displayed at the center of the map.
        /// Note that this LatLng object is not wrapped.
        /// </summary>
        /// <returns></returns>
        public ValueTask<LatLngLiteral> GetCenter()
        {
            return InvokeAsync<LatLngLiteral>("getCenter");
        }

        public ValueTask SetCenter(LatLngLiteral latLng)
        {
            return InvokeVoidAsync("setCenter", latLng);
        }

        /// <summary>
        /// Returns the compass heading of aerial imagery.
        /// The heading value is measured in degrees (clockwise) from cardinal direction North.
        /// </summary>
        /// <returns></returns>
        public ValueTask<int> GetHeading()
        {
            return InvokeAsync<int>("getHeading");
        }

        /// <summary>
        /// Sets the compass heading for aerial imagery measured in degrees from cardinal direction North.
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public ValueTask SetHeading(int heading)
        {
            return InvokeVoidAsync("setHeading", heading);
        }

        public async ValueTask<MapTypeId> GetMapTypeId()
        {
            var mapTypeIdStr = await InvokeAsync<string>("getMapTypeId");

            return Helper.ToEnum<MapTypeId>(mapTypeIdStr);
        }

        public ValueTask SetMapTypeId(MapTypeId mapTypeId)
        {
            return InvokeVoidAsync("setMapTypeId", mapTypeId);
        }

        /// <summary>
        /// Returns the current angle of incidence of the map, in degrees from the viewport plane to the map plane.
        /// The result will be 0 for imagery taken directly overhead or 45 for 45° imagery. 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels.
        /// Note: This method does not return the value set by setTilt.
        /// See setTilt for details.
        /// </summary>
        /// <returns></returns>
        public ValueTask<int> GetTilt()
        {
            return InvokeAsync<int>("getTilt");
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
        public ValueTask SetTilt(int tilt)
        {
            return InvokeVoidAsync("setTilt", tilt);
        }

        public ValueTask<int> GetZoom()
        {
            return InvokeAsync<int>("getZoom");
        }

        public ValueTask SetZoom(int zoom)
        {
            return InvokeVoidAsync("setZoom", zoom);
        }

        public ValueTask SetOptions(MapOptions mapOptions)
        {
            return InvokeVoidAsync("setOptions", mapOptions);
        }
    }
}
