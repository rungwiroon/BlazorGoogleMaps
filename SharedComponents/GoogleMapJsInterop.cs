using Microsoft.JSInterop;
using SharedComponents.Maps;
using System;
using System.Collections.Generic;
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
    }
}
