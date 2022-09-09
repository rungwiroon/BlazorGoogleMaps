using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    
    public class Event
    {
        private readonly IJSRuntime _jsRuntime;

        public Event(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Cross browser event handler registration. 
        /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
        /// </summary>
        public Task AddDomListener(object instance, string eventName, Action handler, bool? capture)
        {
            return _jsRuntime.MyInvokeAsync(
                "google.maps.event.addDomListener", instance, eventName, handler, capture);
        }

        /// <summary>
        /// Cross browser event handler registration. 
        /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
        /// </summary>
        public Task AddDomListener<T>(object instance, string eventName, Action<T> handler, bool? capture)
        {
            return _jsRuntime.MyInvokeAsync(
                "google.maps.event.addDomListener", instance, eventName, handler, capture);
        }
    }
}
