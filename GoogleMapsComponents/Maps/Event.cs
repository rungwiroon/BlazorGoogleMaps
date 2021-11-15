using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Event
    {
        /// <summary>
        /// Cross browser event handler registration. 
        /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
        /// </summary>
        public static ValueTask AddDomListener(
            IJSRuntime jsRuntime,
            IJSObjectReference instance,
            string eventName,
            Action handler,
            bool? capture)
        {
            return jsRuntime.InvokeVoidAsync(
                "google.maps.event.addDomListener",
                instance, eventName, handler, capture);
        }

        /// <summary>
        /// Cross browser event handler registration. 
        /// This listener is removed by calling removeListener(handle) for the handle that is returned by this function.
        /// </summary>
        public static ValueTask AddDomListener<T>(
            IJSRuntime jsRuntime,
            IJSObjectReference instance,
            string eventName,
            Action<T> handler,
            bool? capture)
        {
            return jsRuntime.InvokeVoidAsync(
                "google.maps.event.addDomListener", instance, eventName, handler, capture);
        }

        /// <summary>
        /// Removes all listeners for the given event for the given instance.
        /// </summary>
        public ValueTask ClearListeners(
            IJSRuntime jsRuntime,
            IJSObjectReference instance,
            string eventName)
        {
            return jsRuntime.InvokeVoidAsync(
                "google.maps.event.clearListeners", instance, eventName);
        }
    }
}
