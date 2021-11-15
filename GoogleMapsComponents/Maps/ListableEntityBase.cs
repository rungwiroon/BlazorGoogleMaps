using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OneOf;

namespace GoogleMapsComponents.Maps
{
    public class ListableEntityBase<TEntityOptions> : IDisposable, IJsObjectRef
        where TEntityOptions : ListableEntityOptionsBase
    {
        protected readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public Guid Guid => _jsObjectRef.Guid;
        
        internal ListableEntityBase(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
            EventListeners = new Dictionary<string, List<MapEventListener>>();
        }

        public void Dispose()
        {
            foreach (string key in EventListeners.Keys)
            {
                //Probably superfluous...
                if ((EventListeners.TryGetValue(key, out var eventsList) && eventsList != null))
                {
                    foreach (MapEventListener eventListener in eventsList)
                    {
                        eventListener.Dispose();
                    }

                    eventsList.Clear();
                }
            }

            EventListeners.Clear();
            _jsObjectRef.Dispose();
        }

        public virtual Task<Map> GetMap()
        {
            return _jsObjectRef.InvokeAsync<Map>(
                "getMap");
        }

        /// <summary>
        /// Renders the map entity on the specified map or panorama. 
        /// If map is set to null, the map entity will be removed.
        /// </summary>
        /// <param name="map"></param>
        public virtual async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync("setMap", map);

            //_map = map;
        }

        public virtual async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            JsObjectRef listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }
            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task<MapEventListener> AddListener<V>(string eventName, Action<V> handler)
        {
            JsObjectRef listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }
            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                await _jsObjectRef.InvokeAsync("clearListeners", eventName);

                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead to clear the list and remove the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }

        public Task InvokeAsync(string functionName, params object[] args)
        {
            return _jsObjectRef.InvokeAsync(functionName, args);
        }

        public Task<T> InvokeAsync<T>(string functionName, params object[] args)
        {
            return _jsObjectRef.InvokeAsync<T>(functionName, args);
        }

        public Task<OneOf<T, U>> InvokeAsync<T, U>(string functionName, params object[] args)
        {
            return _jsObjectRef.InvokeAsync<T, U>(functionName, args);
        }

        public Task<OneOf<T, U, V>> InvokeAsync<T, U, V>(string functionName, params object[] args)
        {
            return _jsObjectRef.InvokeAsync<T, U, V>(functionName, args);
        }
    }
}
