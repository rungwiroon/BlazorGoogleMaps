using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// google.maps.MapsEventListener interface
    /// An event listener, created by google.maps.event.addListener() and friends.
    /// </summary>
    public class MapEventListener : JsObjectRef
    {
        internal MapEventListener(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        public ValueTask RemoveAsync()
        {
            return InvokeVoidAsync("remove");
        }
    }
}
