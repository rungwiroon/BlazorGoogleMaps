using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// google.maps.MVCObject class
    /// Base class implementing KVO.
    /// </summary>
    public class MVCObject : JsObjectRef
    {
        internal MVCObject(IJSObjectReference jsObjectRef) : base(jsObjectRef)
        {
        }

        public async ValueTask<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await this.AddListenerAsync(eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async ValueTask<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await this.AddListenerAsync(eventName, handler);

            return new MapEventListener(listenerRef);
        }

        public async ValueTask<MapEventListener> AddListener<T>(string eventName, Func<T, Task> handler)
        {
            var listenerRef = await this.AddListenerAsync(eventName, handler);

            return new MapEventListener(listenerRef);
        }
    }
}
