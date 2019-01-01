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
            return JSRuntime.Current.InvokeAsync<object>(
                "googleMapJsFunctions.init",
                id,
                options);
        }

        public static Task FitBounds(string id, LatLngBoundsLiteral bounds)
        {
            return JSRuntime.Current.InvokeAsync<object>(
                "window._blazorGoogleMaps[id].fitBounds(bounds);");
        }
    }
}
