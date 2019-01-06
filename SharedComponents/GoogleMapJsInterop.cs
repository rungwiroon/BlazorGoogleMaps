using Microsoft.JSInterop;
using SharedComponents.Maps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    public class GoogleMapJsInterop
    {
        public static Task Init(string id, MapOptions options)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.init",
                id,
                options);
        }

        public static Task FitBounds(string id, LatLngBoundsLiteral bounds)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.fitBounds",
                id,
                bounds);
        }

        public static Task PanBy(string id, int x, int y)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.panBy",
                id,
                x,
                y);
        }

        public static Task PanTo(string id, LatLngLiteral latLng)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.panTo",
                id,
                latLng);
        }

        public static Task PanToBounds(string id, LatLngBoundsLiteral latLngBounds)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.panToBounds",
                id,
                latLngBounds);
        }

        public static Task<LatLngBoundsLiteral> GetBounds(string id)
        {
            return Helper.MyInvokeAsync<LatLngBoundsLiteral>(
                "googleMapJsFunctions.getBounds",
                id);
        }

        public static Task<LatLngLiteral> GetCenter(string id)
        {
            return Helper.MyInvokeAsync<LatLngLiteral>(
                "googleMapJsFunctions.getCenter",
                id);
        }

        public static Task SetCenter(string id, LatLngLiteral latLng)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.setCenter",
                id,
                latLng);
        }

        public static Task<bool> GetClickableIcons(string id)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.getClickableIcons",
                id);
        }

        public static Task SetClickableIcons(string id, bool value)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.setClickableIcons",
                id,
                value);
        }

        public static Task<int> GetHeading(string id)
        {
            return Helper.MyInvokeAsync<int>(
                "googleMapJsFunctions.getHeading",
                id);
        }

        public static Task SetHeading(string id, int heading)
        {
            return Helper.MyInvokeAsync<int>(
                "googleMapJsFunctions.setHeading",
                id,
                heading);
        }

        public static async Task<MapTypeId> GetMapTypeId(string id)
        {
            var str = await Helper.MyInvokeAsync<string>(
                "googleMapJsFunctions.getMapTypeId",
                id);

            Debug.WriteLine($"Get map type {str}");

            return Helper.ToEnum<MapTypeId>(str);
        }

        public static Task SetMapTypeId(string id, MapTypeId mapTypeId)
        {
            return Helper.MyInvokeAsync<int>(
                "googleMapJsFunctions.setMapTypeId",
                id,
                mapTypeId);
        }
        
        public static Task<int> GetTilt(string id)
        {
            return Helper.MyInvokeAsync<int>(
                "googleMapJsFunctions.getTilt",
                id);
        }

        public static Task SetTilt(string id, int tilt)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.setTilt",
                id,
                tilt);
        }

        public static Task<int> GetZoom(string id)
        {
            return Helper.MyInvokeAsync<int>(
                "googleMapJsFunctions.getZoom",
                id);
        }

        public static Task SetZoom(string id, int zoom)
        {
            return Helper.MyInvokeAsync<bool>(
                "googleMapJsFunctions.setZoom",
                id,
                zoom);
        }
    }
}
