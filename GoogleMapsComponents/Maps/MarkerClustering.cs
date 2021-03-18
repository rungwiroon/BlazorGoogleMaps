using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// https://googlemaps.github.io/v3-utility-library/modules/_google_markerclustererplus.html
    /// </summary>
    public class MarkerClustering : IJsObjectRef
    {
        private readonly JsObjectRef _jsObjectRef;
        public Guid Guid => _jsObjectRef.Guid;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public static async Task<MarkerClustering> CreateAsync(
            IJSRuntime jsRuntime,
            Map map,
            IEnumerable<Marker> markers,
            string imagePath = "_content/BlazorGoogleMaps/m")
        {
            var guid = System.Guid.NewGuid();
            var jsObjectRef = new JsObjectRef(jsRuntime, guid);
            await jsRuntime.InvokeVoidAsync("googleMapsObjectManager.addClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, imagePath);
            var obj = new MarkerClustering(jsObjectRef);
            return obj;
        }

        internal MarkerClustering(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
            EventListeners = new Dictionary<string, List<MapEventListener>>();
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

        public virtual async Task SetMap(Map map)
        {
            await _jsObjectRef.InvokeAsync("setMap", map);
        }

        public virtual async Task ClearMarkers()
        {
            await _jsObjectRef.InvokeAsync("clearMarkers");
        }

        public virtual async Task FitMapToMarkers(int padding)
        {
            await _jsObjectRef.InvokeAsync("fitMapToMarkers", padding);
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
    }
}