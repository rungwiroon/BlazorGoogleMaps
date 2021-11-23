using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// google.maps.MapsEventListener interface
    /// An event listener, created by google.maps.event.addListener() and friends.
    /// </summary>
    public class MapEventListener : Object
    {
        private readonly IDisposable callbackObjRef;

        internal MapEventListener(IJSObjectReference jsObjectRef, IDisposable callbackObjRef)
            : base(jsObjectRef)
        {
            this.callbackObjRef = callbackObjRef;
        }

        public async ValueTask RemoveAsync()
        {
            await this.InvokeVoidAsync("remove");
            callbackObjRef.Dispose();
        }
    }
}
